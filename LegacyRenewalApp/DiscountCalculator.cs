using System;
using System.Collections.Generic;

namespace LegacyRenewalApp
{
    internal class DiscountCalculator
    {
        private readonly IReadOnlyCollection<IDiscountRule> _rules;

        public DiscountCalculator(IReadOnlyCollection<IDiscountRule> rules)
        {
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
        }

        public decimal Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            decimal baseAmount,
            bool useLoyaltyPoints,
            PricingNotes notes)
        {
            decimal totalDiscount = 0m;

            foreach (var rule in _rules)
            {
                totalDiscount += rule.Calculate(customer, plan, seatCount, baseAmount, useLoyaltyPoints, notes);
            }

            return totalDiscount;
        }
    }
}
