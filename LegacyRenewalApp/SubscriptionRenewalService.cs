using System;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerLookup _customerLookup;
        private readonly ISubscriptionPlanLookup _planLookup;
        private readonly IRenewalPricingCalculator _pricingCalculator;
        private readonly IRenewalInvoiceFactory _invoiceFactory;
        private readonly IBillingGateway _billingGateway;

        public SubscriptionRenewalService()
            : this(
                new CustomerRepositoryLookup(new CustomerRepository()),
                new SubscriptionPlanRepositoryLookup(new SubscriptionPlanRepository()),
                new RenewalPricingCalculator(
                    new DiscountCalculator(new IDiscountRule[]
                    {
                        new SegmentDiscountRule(),
                        new LoyaltyTenureDiscountRule(),
                        new TeamSizeDiscountRule(),
                        new LoyaltyPointsDiscountRule()
                    }),
                    new PremiumSupportFeeCalculator(),
                    new PaymentFeeCalculator(new IPaymentFeePolicy[]
                    {
                        new CardPaymentFeePolicy(),
                        new BankTransferPaymentFeePolicy(),
                        new PayPalPaymentFeePolicy(),
                        new InvoicePaymentFeePolicy()
                    }),
                    new TaxRateProvider()),
                new RenewalInvoiceFactory(),
                new LegacyBillingGatewayAdapter())
        {
        }

        public SubscriptionRenewalService(
            ICustomerLookup customerLookup,
            ISubscriptionPlanLookup planLookup,
            IRenewalPricingCalculator pricingCalculator,
            IRenewalInvoiceFactory invoiceFactory,
            IBillingGateway billingGateway)
        {
            _customerLookup = customerLookup ?? throw new ArgumentNullException(nameof(customerLookup));
            _planLookup = planLookup ?? throw new ArgumentNullException(nameof(planLookup));
            _pricingCalculator = pricingCalculator ?? throw new ArgumentNullException(nameof(pricingCalculator));
            _invoiceFactory = invoiceFactory ?? throw new ArgumentNullException(nameof(invoiceFactory));
            _billingGateway = billingGateway ?? throw new ArgumentNullException(nameof(billingGateway));
        }

        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            var request = RenewalRequestValidator.ValidateAndNormalize(
                customerId,
                planCode,
                seatCount,
                paymentMethod);

            var customer = _customerLookup.GetById(request.CustomerId);
            var plan = _planLookup.GetByCode(request.NormalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }

            var pricingResult = _pricingCalculator.Calculate(
                customer,
                plan,
                request.SeatCount,
                request.NormalizedPlanCode,
                request.NormalizedPaymentMethod,
                includePremiumSupport,
                useLoyaltyPoints);

            var invoice = _invoiceFactory.Create(
                request.CustomerId,
                customer,
                request.NormalizedPlanCode,
                request.NormalizedPaymentMethod,
                request.SeatCount,
                pricingResult);

            _billingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {request.NormalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                _billingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}
