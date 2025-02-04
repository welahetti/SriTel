using Customers.Application;
using Microsoft.AspNetCore.Mvc;
using Mock.Provisioning.Service;
using SriTel.Billing.Application.Services.Interfaces;

namespace Customer.API
{
    [ApiController]
    [Route("api/services")]
    public class ServiceController : ControllerBase
    {
        private readonly IBillingService _billingService;
        private readonly ICustomerService _customerService;
        private readonly IProvisioningService _provisioningService;

        public ServiceController(IBillingService billingService, ICustomerService customerService, IProvisioningService provisioningService)
        {
            _billingService = billingService;
            _customerService = customerService;
            _provisioningService = provisioningService;
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateService([FromBody] ServiceRequest serviceRequest)
        {
            // Check if user exists
            var user = await _customerService.GetCustomerByIdAsync(serviceRequest.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Check if the user has any outstanding bills
            var bills = await _billingService.GetBillsByUserAsync(serviceRequest.UserId);
            if (bills.Any(b => !b.IsPaid)) // Check for unpaid bills
            {
                return BadRequest(new { Message = "User has unpaid bills. Service cannot be activated." });
            }

            // Simulate service activation logic (mock call to Provisioning System)
            var activationResult = await _provisioningService.ActivateServiceAsync(serviceRequest);
            if (activationResult.IsSuccess)
            {
                return Ok(new { Message = $"Service {serviceRequest.ServiceName} activated successfully." });
            }

            return BadRequest(new { Message = "Service activation failed." });
        }

        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateService([FromBody] ServiceRequest serviceRequest)
        {
            // Check if user exists
            var user = await _customerService.GetCustomerByIdAsync(serviceRequest.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Call the mock provisioning API to deactivate the service
            var deactivationResult = await _provisioningService.DeactivateServiceAsync(serviceRequest);
            if (deactivationResult.IsSuccess)
            {
                return Ok(new { Message = $"Service {serviceRequest.ServiceName} deactivated successfully." });
            }

            return BadRequest(new { Message = "Service deactivation failed." });
        }
    }

   

    
}
