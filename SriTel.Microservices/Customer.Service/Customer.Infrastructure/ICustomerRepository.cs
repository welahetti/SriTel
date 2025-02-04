using Customers.Domain;

namespace Customers.Infrastructure
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Domain.Customer customer);
        Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId);
        Task<Domain.Customer> GetCustomerByEmailAsync(string email);
        Task<Domain.Customer> GetCustomerByIdAsync(Guid id);
        Task UpdateCustomerAsync(Domain.Customer customer);
    }
}
