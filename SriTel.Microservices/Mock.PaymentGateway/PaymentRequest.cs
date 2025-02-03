namespace Mock.PaymentGateway
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string TransactionId { get; set; }

        public string PaymentMethod { get; set; }
    }

}
