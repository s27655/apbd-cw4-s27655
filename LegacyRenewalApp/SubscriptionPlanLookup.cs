using System;

namespace LegacyRenewalApp
{
    public interface ISubscriptionPlanLookup
    {
        SubscriptionPlan GetByCode(string code);
    }

    internal class SubscriptionPlanRepositoryLookup : ISubscriptionPlanLookup
    {
        private readonly SubscriptionPlanRepository _repository;

        public SubscriptionPlanRepositoryLookup(SubscriptionPlanRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public SubscriptionPlan GetByCode(string code)
        {
            return _repository.GetByCode(code);
        }
    }
}
