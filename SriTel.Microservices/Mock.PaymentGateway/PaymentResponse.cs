namespace Mock.PaymentGateway
{
    public class PaymentResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; } // "Success" or "Failed"
        public string GatewayReference { get; set; }
    }
}
