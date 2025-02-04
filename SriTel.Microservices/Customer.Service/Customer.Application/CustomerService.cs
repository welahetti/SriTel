using Customer.Service.Customer.API.DTO;
using Customers.Domain;
using Customers.Infrastructure;
using Microsoft.AspNetCore.Identity.Data;
using System.Text.Json;

namespace Customers.Application
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly HttpClient _httpClient;
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

            var customer = new Domain.Customer
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

        public async Task<Domain.Customer> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            return customer;
        }

        public async Task<IEnumerable<Bill>>  GetBillsByUserAsync(Guid userId)
        {
            var bills = await _customerRepository.GetBillsByUserAsync(userId);

            return bills.Select(b => new Bill
            {
                BillID = b.BillID,
                Amount = b.Amount,
                DueDate = b.DueDate,
                IsPaid = b.IsPaid
            });

        }

        public async Task<bool> ActivateServiceAsync(ProvisionServiceRequest serviceRequest)
        {
            var provisionserviceRequest = new
            {
                UserId = serviceRequest.UserId,
                ServiceName = serviceRequest.ServiceName 
            };

            var content = new StringContent(JsonSerializer.Serialize(serviceRequest), System.Text.Encoding.UTF8, "application/json");

            // Call Mock.PaymentGateway API to process payment
            var response = await _httpClient.PostAsync("/api/provisioning/service/activate", content);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response content
                var responseContent = await response.Content.ReadAsStringAsync();
                var provisionResponse = JsonSerializer.Deserialize<ProvisioningResult>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (provisionResponse!=null && provisionResponse?.IsSuccess == true) // Check if IsSuccess is true
                {
                    return true; // Service provisioned successfully
                }
            }

            return false; // Payment failed
        }

       

        public async  Task<bool> DeactivateServiceAsync(ProvisionServiceRequest serviceRequest)
        {
            var provisionserviceRequest = new
            {
                UserId = serviceRequest.UserId,
                ServiceName = serviceRequest.ServiceName
            };

            var content = new StringContent(JsonSerializer.Serialize(serviceRequest), System.Text.Encoding.UTF8, "application/json");

            // Call Mock.PaymentGateway API to process payment
            var response = await _httpClient.PostAsync("/api/provisioning/service/deactivate", content);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response content
                var responseContent = await response.Content.ReadAsStringAsync();
                var provisionResponse = JsonSerializer.Deserialize<ProvisioningResult>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (provisionResponse != null && provisionResponse?.IsSuccess == true) // Check if IsSuccess is true
                {
                    return true; // Service provisioned successfully
                }
            }

            return false; // Payment failed
        }

    }

}
