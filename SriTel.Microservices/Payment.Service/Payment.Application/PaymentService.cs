using Payments.Infrastructure;
using Payments.Domain;
namespace Payments.Application
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);
            return payments.Select(payment => new Payment
            {
                PaymentID = payment.PaymentID,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate
            });
        }
    }
}
