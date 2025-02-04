
using Billing.Service.Billing.Infrastructure;
using Customers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public Task AddCustomerAsync(Domain.Customer customer)
        {
            _context.Add(customer);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId)
        {
            return await _context.Bills.Where(b => b.UserID == userId).ToListAsync();
        }

        public Task<Domain.Customer> GetCustomerByEmailAsync(string email)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            return Task.FromResult(customer);
        }

        public Task<Domain.Customer> GetCustomerByIdAsync(Guid id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            return Task.FromResult(customer);
        }

        public Task UpdateCustomerAsync(Domain.Customer customer)
        {
            var existing = _context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
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
