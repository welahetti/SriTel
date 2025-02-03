using Microsoft.AspNetCore.Mvc;
using Payment.Service.Payment.API.DTO;
using Payments.Application;
using System;
using System.Threading.Tasks;

namespace Payments.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        public PaymentController(IPaymentService paymentService, IHttpClientFactory httpClientFactory)
        {            
            _httpClient = httpClientFactory.CreateClient("PaymentGateway");
            _paymentService = paymentService;
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

        /// <summary>
        /// Gets the list of payments by Bill ID.
        /// </summary>
        /// <param name="billId">The ID of the bill whose payments are retrieved.</param>
        /// <returns>A list of payments for the specified bill.</returns>
        [HttpGet("bill/{billId}")]
        public async Task<IActionResult> GetPaymentsByBillId(Guid billId)
        {
            var payments = await _paymentService.GetPaymentsByBillIdAsync(billId);

            if (payments == null || !payments.Any())
            {
                return NotFound(new { Message = $"No payments found for Bill ID: {billId}" });
            }

            return Ok(payments);
        }

        /// <summary>
        /// Processes a payment for a bill.
        /// </summary>
        /// <param name="request">The payment request containing card number and amount.</param>
        /// <returns>A response indicating whether the payment was successful.</returns>
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.CardNumber) || request.Amount <= 0)
            {
                return BadRequest(new { Message = "Invalid payment request." });
            }

            bool result = await _paymentService.ProcessPaymentAsync(request.CardNumber, request.Amount,request.PaymentMethod,request.BillID);

            if (result)
            {
                return Ok(new { Message = "Payment processed successfully." });
            }

            return BadRequest(new { Message = "Payment processing failed." });
        }

        /// <summary>
        /// Checks the status of a payment.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to check.</param>
        /// <returns>The status of the payment.</returns>
        [HttpGet("status/{transactionId}")]
        public async Task<IActionResult> CheckPaymentStatus(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                return BadRequest(new { Message = "Transaction ID is required." });
            }

            string status = await _paymentService.CheckPaymentStatusAsync(transactionId);

            if (status == "Failed to retrieve payment status")
            {
                return NotFound(new { Message = "Payment status could not be retrieved." });
            }

            return Ok(new { Status = status });
        }
    }

  
}