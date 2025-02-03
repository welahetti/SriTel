namespace Payment.Service.Payment.API.DTO
{
    public class ProcessPaymentResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string GatewayReference { get; set; }
    }
}
