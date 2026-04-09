namespace LegacyRenewalApp
{
    internal interface IDiscountRule
    {
        decimal Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            decimal baseAmount,
            bool useLoyaltyPoints,
            PricingNotes notes);
    }

    internal class SegmentDiscountRule : IDiscountRule
    {
        public decimal Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            decimal baseAmount,
            bool useLoyaltyPoints,
            PricingNotes notes)
        {
            if (customer.Segment == "Silver")
            {
                notes.Add("silver discount");
                return baseAmount * 0.05m;
            }

            if (customer.Segment == "Gold")
            {
                notes.Add("gold discount");
                return baseAmount * 0.10m;
            }

            if (customer.Segment == "Platinum")
            {
                notes.Add("platinum discount");
                return baseAmount * 0.15m;
            }

            if (customer.Segment == "Education" && plan.IsEducationEligible)
            {
                notes.Add("education discount");
                return baseAmount * 0.20m;
            }

            return 0m;
        }
    }

    internal class LoyaltyTenureDiscountRule : IDiscountRule
    {
        public decimal Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            decimal baseAmount,
            bool useLoyaltyPoints,
            PricingNotes notes)
        {
            if (customer.YearsWithCompany >= 5)
            {
                notes.Add("long-term loyalty discount");
                return baseAmount * 0.07m;
            }

            if (customer.YearsWithCompany >= 2)
            {
                notes.Add("basic loyalty discount");
                return baseAmount * 0.03m;
            }

            return 0m;
        }
    }

    internal class TeamSizeDiscountRule : IDiscountRule
    {
        public decimal Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            decimal baseAmount,
            bool useLoyaltyPoints,
            PricingNotes notes)
        {
            if (seatCount >= 50)
            {
                notes.Add("large team discount");
                return baseAmount * 0.12m;
            }

            if (seatCount >= 20)
            {
                notes.Add("medium team discount");
                return baseAmount * 0.08m;
            }

            if (seatCount >= 10)
            {
                notes.Add("small team discount");
                return baseAmount * 0.04m;
            }

            return 0m;
        }
    }

    internal class LoyaltyPointsDiscountRule : IDiscountRule
    {
        public decimal Calculate(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            decimal baseAmount,
            bool useLoyaltyPoints,
            PricingNotes notes)
        {
            if (!useLoyaltyPoints || customer.LoyaltyPoints <= 0)
            {
                return 0m;
            }

            int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
            notes.Add($"loyalty points used: {pointsToUse}");
            return pointsToUse;
        }
    }
}
