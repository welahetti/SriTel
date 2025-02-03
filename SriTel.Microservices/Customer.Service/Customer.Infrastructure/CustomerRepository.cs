
using Customers.Domain;

namespace Customers.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly List<Domain.Customer> _customers = new(); // Mock database

        public Task AddCustomerAsync(Domain.Customer customer)
        {
            _customers.Add(customer);
            return Task.CompletedTask;
        }

        public Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var customer = _customers.FirstOrDefault(c => c.Email == email);
            return Task.FromResult(customer);
        }

        public Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == id);
            return Task.FromResult(customer);
        }

        public Task UpdateCustomerAsync(Customer customer)
        {
            var existing = _customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (existing != null)
            {
                existing.FullName = customer.FullName;
                existing.PhoneNumber = customer.PhoneNumber;
                existing.PasswordHash = customer.PasswordHash;
            }
            return Task.CompletedTask;
        }
    }

}
