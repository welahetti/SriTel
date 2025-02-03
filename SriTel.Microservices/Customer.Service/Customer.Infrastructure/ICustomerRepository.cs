using Customers.Domain;

namespace Customers.Infrastructure
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task UpdateCustomerAsync(Customer customer);
    }
}
