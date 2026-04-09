using System;

namespace LegacyRenewalApp
{
    public interface ICustomerLookup
    {
        Customer GetById(int customerId);
    }

    internal class CustomerRepositoryLookup : ICustomerLookup
    {
        private readonly CustomerRepository _repository;

        public CustomerRepositoryLookup(CustomerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Customer GetById(int customerId)
        {
            return _repository.GetById(customerId);
        }
    }
}
