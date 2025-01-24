using Payment.Infrastructure;
using Payment.Domain;
namespace Payment.Application
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<Pay>> GetPaymentsByUserIdAsync(Guid userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);
            return payments.Select(payment => new Pay
            {
                PaymentID = payment.PaymentID,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate
            });
        }
    }
}
