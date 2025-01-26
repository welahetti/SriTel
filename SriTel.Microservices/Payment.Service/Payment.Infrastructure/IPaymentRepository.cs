using Payments.Domain;

namespace Payments.Infrastructure
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Domain.Payment>> GetPaymentsByBillIdAsync(Guid billId);
        Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
