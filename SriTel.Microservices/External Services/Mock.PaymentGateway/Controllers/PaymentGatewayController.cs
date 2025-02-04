using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Mock.PaymentGateway.Controllers
{   

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentGatewayController : ControllerBase
    {
        private static readonly Dictionary<string, PaymentStatus> Transactions = new();

        [HttpPost("payments")]
        public ActionResult<PaymentResponse> ProcessPayment([FromBody] PaymentRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.TransactionId) || string.IsNullOrEmpty(request.CardNumber) || request.Amount <= 0)
            {
                return BadRequest(new { Message = "Invalid payment request. Ensure all fields are provided." });
            }

            // Extract the last 4 digits of the card number
            string lastFourDigits = request.CardNumber.Length >= 4
                ? request.CardNumber.Substring(request.CardNumber.Length - 4)
                : request.CardNumber;

            // Simulate success or failure randomly
            var random = new Random();
            var status = random.Next(0, 2) == 0 ? "Success" : "Failed";

            var paymentStatus = new PaymentStatus
            {
                TransactionId = request.TransactionId,
                Status = status,
                GatewayReference = Guid.NewGuid().ToString()
            };

            // Save the transaction details
            Transactions[request.TransactionId] = paymentStatus;

            return Ok(new PaymentResponse
            {
                TransactionId = request.TransactionId,
                Status = paymentStatus.Status,
                GatewayReference = paymentStatus.GatewayReference,
                LastFourDigits = lastFourDigits
            });
        }



        [HttpGet("payments/{transactionId}")]
        public ActionResult<PaymentStatus> GetPaymentStatus(string transactionId)
        {
            if (Transactions.TryGetValue(transactionId, out var paymentStatus))
            {
                return Ok(paymentStatus);
            }

            return NotFound(new { Message = "Transaction not found" });
        }
    }

}
