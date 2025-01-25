using Payments.Domain;

namespace Payments.Infrastructure
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetPaymentsByBillIdAsync(Guid billId);
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
