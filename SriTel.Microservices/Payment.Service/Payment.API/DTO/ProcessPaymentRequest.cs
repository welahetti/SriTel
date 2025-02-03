
namespace Payment.Service.Payment.API.DTO
{
    /// <summary>
    /// Represents a payment request for processing.
    /// </summary>
    public class ProcessPaymentRequest
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public Guid BillID { get;  set; }
    }
}
