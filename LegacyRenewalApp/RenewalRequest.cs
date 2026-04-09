namespace LegacyRenewalApp
{
    internal class RenewalRequest
    {
        public RenewalRequest(int customerId, string normalizedPlanCode, int seatCount, string normalizedPaymentMethod)
        {
            CustomerId = customerId;
            NormalizedPlanCode = normalizedPlanCode;
            SeatCount = seatCount;
            NormalizedPaymentMethod = normalizedPaymentMethod;
        }

        public int CustomerId { get; }

        public string NormalizedPlanCode { get; }

        public int SeatCount { get; }

        public string NormalizedPaymentMethod { get; }
    }
}
