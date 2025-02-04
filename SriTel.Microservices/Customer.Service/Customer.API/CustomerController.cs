using Customer.Service.Customer.API.DTO;
using Customers.Application;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Mock.Provisioning.Service;

namespace Customers.API
{
    [ApiController]
    [Route("api/Customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
      
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _customerService.RegisterCustomerAsync(request);
                return Ok(new { Message = "Customer registered successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (await _customerService.AuthenticateCustomerAsync(request.Email, request.Password))
            {
                return Ok(new { Message = "Login successful." });
            }
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                await _customerService.ResetPasswordAsync(request);
                return Ok(new { Message = "Password reset successful." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                await _customerService.UpdateProfileAsync(request);
                return Ok(new { Message = "Profile updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                return Ok(customer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpPost("activate")]
        public async Task<IActionResult> ActivateService([FromBody] ProvisionServiceRequest serviceRequest)
        {
            // Check if user exists
            var user = await _customerService.GetCustomerByIdAsync(serviceRequest.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            //// Check if the user has any outstanding bills
            //var bills = await _customerService.GetBillsByUserAsync(serviceRequest.UserId);

            //var unpaidBills = bills.Select(b => new { b.Id, b.IsPaid }).Where(b => !b.IsPaid);

            //if (unpaidBills.Any()) // Check if any unpaid bills exist
            //{
            //    return BadRequest(new { Message = "User has unpaid bills. Service cannot be activated." });
            //}



            // Simulate service activation logic (mock call to Provisioning System)
            var activationResult = await _customerService.ActivateServiceAsync(serviceRequest);
            if (activationResult)
            {
                return Ok(new { Message = $"Service {serviceRequest.ServiceName} activated successfully." });
            }

            return BadRequest(new { Message = "Service activation failed." });
        }

        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateService([FromBody] ProvisionServiceRequest serviceRequest)
        {
            // Check if user exists
            var user = await _customerService.GetCustomerByIdAsync(serviceRequest.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Call the mock provisioning API to deactivate the service
            var deactivationResult = await _customerService.DeactivateServiceAsync(serviceRequest);
            if (deactivationResult)
            {
                return Ok(new { Message = $"Service {serviceRequest.ServiceName} deactivated successfully." });
            }

            return BadRequest(new { Message = "Service deactivation failed." });
        }
    }

}
