
using Payments.Domain;

namespace Payments.Application
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPaymentsByBillIdAsync(Guid billId);
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
