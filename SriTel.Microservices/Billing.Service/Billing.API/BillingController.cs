using Microsoft.AspNetCore.Mvc;
using SriTel.Billing.Application.Services;
using SriTel.Billing.Application.Services.Interfaces;

namespace Billing.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : ControllerBase
    {

        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBillsByUserAsync(Guid userId)
        {
            var bills = await _billingService.GetBillsByUserAsync(userId);
            return Ok(bills);
        }
       
    }
}

