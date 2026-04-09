namespace LegacyRenewalApp
{
    public interface IRenewalPricingCalculator
    {
        RenewalPricingResult Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            string normalizedPlanCode,
            string normalizedPaymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints);
    }

    internal class RenewalPricingCalculator : IRenewalPricingCalculator
    {
        private readonly DiscountCalculator _discountCalculator;
        private readonly PremiumSupportFeeCalculator _supportFeeCalculator;
        private readonly PaymentFeeCalculator _paymentFeeCalculator;
        private readonly TaxRateProvider _taxRateProvider;

        public RenewalPricingCalculator(
            DiscountCalculator discountCalculator,
            PremiumSupportFeeCalculator supportFeeCalculator,
            PaymentFeeCalculator paymentFeeCalculator,
            TaxRateProvider taxRateProvider)
        {
            _discountCalculator = discountCalculator;
            _supportFeeCalculator = supportFeeCalculator;
            _paymentFeeCalculator = paymentFeeCalculator;
            _taxRateProvider = taxRateProvider;
        }

        public RenewalPricingResult Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            string normalizedPlanCode,
            string normalizedPaymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            var notes = new PricingNotes();

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;

            decimal discountAmount = _discountCalculator.Calculate(
                customer,
                plan,
                seatCount,
                baseAmount,
                useLoyaltyPoints,
                notes);

            decimal subtotalAfterDiscount = baseAmount - discountAmount;
            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                notes.Add("minimum discounted subtotal applied");
            }

            decimal supportFee = _supportFeeCalculator.Calculate(includePremiumSupport, normalizedPlanCode, notes);
            decimal paymentFee = _paymentFeeCalculator.Calculate(normalizedPaymentMethod, subtotalAfterDiscount, supportFee, notes);

            decimal taxRate = _taxRateProvider.GetTaxRate(customer.Country);
            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = taxBase * taxRate;
            decimal finalAmount = taxBase + taxAmount;

            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                notes.Add("minimum invoice amount applied");
            }

            return new RenewalPricingResult
            {
                BaseAmount = baseAmount,
                DiscountAmount = discountAmount,
                SupportFee = supportFee,
                PaymentFee = paymentFee,
                TaxAmount = taxAmount,
                FinalAmount = finalAmount,
                Notes = notes.ToString()
            };
        }
    }
}
