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
                GatewayReference = paymentStatus.GatewayReference
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
