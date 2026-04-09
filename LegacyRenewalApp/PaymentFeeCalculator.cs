using System;
using System.Collections.Generic;

namespace LegacyRenewalApp
{
    internal class PaymentFeeCalculator
    {
        private readonly IReadOnlyCollection<IPaymentFeePolicy> _policies;

        public PaymentFeeCalculator(IReadOnlyCollection<IPaymentFeePolicy> policies)
        {
            _policies = policies ?? throw new ArgumentNullException(nameof(policies));
        }

        public decimal Calculate(string normalizedPaymentMethod, decimal subtotalAfterDiscount, decimal supportFee, PricingNotes notes)
        {
            foreach (var policy in _policies)
            {
                if (policy.CanHandle(normalizedPaymentMethod))
                {
                    return policy.Calculate(subtotalAfterDiscount, supportFee, notes);
                }
            }

            throw new ArgumentException("Unsupported payment method");
        }
    }
}
