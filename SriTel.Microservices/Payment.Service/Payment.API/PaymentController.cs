using Microsoft.AspNetCore.Mvc;
using Payments.Application;

namespace Payments.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {

        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService _paymentService)
        {
            _paymentService = _paymentService;
        }

        /// <summary>
        /// Gets the list of payments by User ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose payments are retrieved.</param>
        /// <returns>A list of payments for the specified user.</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserId(Guid userId)
        {
            var payments = await _paymentService.GetPaymentsByUserIdAsync(userId);
            if (payments == null || !payments.Any())
            {
                return NotFound("No payments found for the user.");
            }
            return Ok(payments);
        }
    }
}

