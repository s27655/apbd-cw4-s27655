namespace LegacyRenewalApp
{
    internal interface IPaymentFeePolicy
    {
        bool CanHandle(string normalizedPaymentMethod);

        decimal Calculate(decimal subtotalAfterDiscount, decimal supportFee, PricingNotes notes);
    }

    internal class CardPaymentFeePolicy : IPaymentFeePolicy
    {
        public bool CanHandle(string normalizedPaymentMethod) => normalizedPaymentMethod == "CARD";

        public decimal Calculate(decimal subtotalAfterDiscount, decimal supportFee, PricingNotes notes)
        {
            notes.Add("card payment fee");
            return (subtotalAfterDiscount + supportFee) * 0.02m;
        }
    }

    internal class BankTransferPaymentFeePolicy : IPaymentFeePolicy
    {
        public bool CanHandle(string normalizedPaymentMethod) => normalizedPaymentMethod == "BANK_TRANSFER";

        public decimal Calculate(decimal subtotalAfterDiscount, decimal supportFee, PricingNotes notes)
        {
            notes.Add("bank transfer fee");
            return (subtotalAfterDiscount + supportFee) * 0.01m;
        }
    }

    internal class PayPalPaymentFeePolicy : IPaymentFeePolicy
    {
        public bool CanHandle(string normalizedPaymentMethod) => normalizedPaymentMethod == "PAYPAL";

        public decimal Calculate(decimal subtotalAfterDiscount, decimal supportFee, PricingNotes notes)
        {
            notes.Add("paypal fee");
            return (subtotalAfterDiscount + supportFee) * 0.035m;
        }
    }

    internal class InvoicePaymentFeePolicy : IPaymentFeePolicy
    {
        public bool CanHandle(string normalizedPaymentMethod) => normalizedPaymentMethod == "INVOICE";

        public decimal Calculate(decimal subtotalAfterDiscount, decimal supportFee, PricingNotes notes)
        {
            notes.Add("invoice payment");
            return 0m;
        }
    }
}
