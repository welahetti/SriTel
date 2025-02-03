using Customers.Application.DTO;
using Customers.Domain;
using Customers.Infrastructure;
using Microsoft.AspNetCore.Identity.Data;

namespace Customers.Application
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> RegisterCustomerAsync(RegisterRequest request)
        {
            if (await _customerRepository.GetCustomerByEmailAsync(request.Email) != null)
            {
                throw new InvalidOperationException("Customer with this email already exists.");
            }

            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = string.Empty,
                Email = request.Email,
                PhoneNumber = string.Empty,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _customerRepository.AddCustomerAsync(customer);
            return true;
        }

        public async Task<bool> AuthenticateCustomerAsync(string email, string password)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (customer == null || !BCrypt.Net.BCrypt.Verify(password, customer.PasswordHash))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _customerRepository.UpdateCustomerAsync(customer);
            return true;
        }

        public async Task<bool> UpdateProfileAsync(UpdateProfileRequest request)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            customer.FullName = request.FullName;
            customer.PhoneNumber = request.PhoneNumber;
            await _customerRepository.UpdateCustomerAsync(customer);
            return true;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            return customer;
        }
    }

}
