using Customer.Service.Customer.API.DTO;
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
        Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId);
        Task<bool> ActivateServiceAsync(ProvisionServiceRequest serviceRequest);
        Task<bool> DeactivateServiceAsync(ProvisionServiceRequest serviceRequest);

    }

}
