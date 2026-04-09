namespace LegacyRenewalApp
{
    internal class PremiumSupportFeeCalculator
    {
        public decimal Calculate(bool includePremiumSupport, string normalizedPlanCode, PricingNotes notes)
        {
            if (!includePremiumSupport)
            {
                return 0m;
            }

            decimal supportFee = 0m;
            if (normalizedPlanCode == "START")
            {
                supportFee = 250m;
            }
            else if (normalizedPlanCode == "PRO")
            {
                supportFee = 400m;
            }
            else if (normalizedPlanCode == "ENTERPRISE")
            {
                supportFee = 700m;
            }

            notes.Add("premium support included");
            return supportFee;
        }
    }
}
