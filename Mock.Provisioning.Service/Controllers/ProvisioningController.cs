using Microsoft.AspNetCore.Mvc;

namespace Mock.Provisioning.Service.Controllers
{
    [ApiController]
    [Route("api/provisioning")]
    public class ProvisioningController : ControllerBase
    {
        // Mock activation of a service
        [HttpPost("service/activate")]
        public async Task<IActionResult> ActivateService([FromBody] ServiceRequest serviceRequest)
        {
            if (serviceRequest == null || string.IsNullOrEmpty(serviceRequest.ServiceName))
            {
                return BadRequest(new { Message = "Service name is required." });
            }

            // Simulate a call to the Provisioning System to activate the service
            await Task.Delay(500); // Simulate some delay for activation

            // Mock successful activation
            return Ok(new { Message = $"Service {serviceRequest.ServiceName} activated successfully for user {serviceRequest.UserId}" });
        }

        // Mock deactivation of a service
        [HttpPost("service/deactivate")]
        public async Task<IActionResult> DeactivateService([FromBody] ServiceRequest serviceRequest)
        {
            if (serviceRequest == null || string.IsNullOrEmpty(serviceRequest.ServiceName))
            {
                return BadRequest(new { Message = "Service name is required." });
            }

            // Simulate a call to the Provisioning System to deactivate the service
            await Task.Delay(500); // Simulate some delay for deactivation

            // Mock successful deactivation
            return Ok(new { Message = $"Service {serviceRequest.ServiceName} deactivated successfully for user {serviceRequest.UserId}" });
        }
    }

    public class ServiceRequest
    {
        public Guid UserId { get; set; }
        public string ServiceName { get; set; }
    }
}
