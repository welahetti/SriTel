using Billing.Domain;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{billId}")]
        public async Task<IActionResult> GetBillByIdAsync(Guid billId)
        {
            if (billId == Guid.Empty)
                return BadRequest("Invalid billId provided.");

            try
            {
                var bill = await _billingService.GetBillByBillIdAsync(billId);

                if (bill == null)
                    return NotFound($"No bill found with ID: {billId}");

                return Ok(bill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // POST: api/bills
        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] Bill bill)
        {
            if (bill == null)
            {
                return BadRequest("Bill data is required.");
            }

            try
            {
                // Create the bill asynchronously
                await _billingService.CreateBillAsync(bill);

                // Return a success response with the BillID (or other details if needed)
                return CreatedAtAction(nameof(CreateBill), new { billId = bill.BillID }, bill);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

