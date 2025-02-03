using Customers.Application.DTO;
using Customers.Domain;
using Microsoft.AspNetCore.Identity.Data;

namespace Customers.Application
{
    public interface ICustomerService
    {
        Task<bool> RegisterCustomerAsync(RegisterRequest request);
        Task<bool> AuthenticateCustomerAsync(string email, string password);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task<bool> UpdateProfileAsync(UpdateProfileRequest request);
        Task<Domain.Customer> GetCustomerByIdAsync(Guid id);
    }

}
