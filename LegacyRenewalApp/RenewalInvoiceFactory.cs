using System;

namespace LegacyRenewalApp
{
    public interface IRenewalInvoiceFactory
    {
        RenewalInvoice Create(
            int customerId,
            Customer customer,
            string normalizedPlanCode,
            string normalizedPaymentMethod,
            int seatCount,
            RenewalPricingResult pricingResult);
    }

    internal class RenewalInvoiceFactory : IRenewalInvoiceFactory
    {
        public RenewalInvoice Create(
            int customerId,
            Customer customer,
            string normalizedPlanCode,
            string normalizedPaymentMethod,
            int seatCount,
            RenewalPricingResult pricingResult)
        {
            return new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(pricingResult.BaseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(pricingResult.DiscountAmount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(pricingResult.SupportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(pricingResult.PaymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(pricingResult.TaxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(pricingResult.FinalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = pricingResult.Notes,
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}
