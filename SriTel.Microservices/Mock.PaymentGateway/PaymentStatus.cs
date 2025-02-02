namespace Mock.PaymentGateway
{
    public class PaymentStatus
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string GatewayReference { get; set; }
    }
}
