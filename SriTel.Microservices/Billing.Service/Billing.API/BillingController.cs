using Billing.Application.DTO;
using Billing.Domain;
using Billing.Service.Billing.Application.DTO;
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
        [Route("create")]
        public async Task<IActionResult> CreateBill([FromBody] CreateBillDTO createBillDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map DTO to domain model
            var bill = new Bill
            {
                BillID = Guid.NewGuid(), // Generate a new ID
                UserID = createBillDTO.UserID,
                Amount = createBillDTO.Amount,
                DueDate = createBillDTO.DueDate,
                BillingDate = DateTime.UtcNow, // Set the billing date to current UTC
                IsPaid = false
            };

            await _billingService.CreateBillAsync(bill);

            return Ok(new { Message = "Bill created successfully", BillID = bill.BillID });
        }
    }

}


