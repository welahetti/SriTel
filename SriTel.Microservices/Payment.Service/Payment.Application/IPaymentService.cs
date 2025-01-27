
using Payments.Domain;

namespace Payments.Application
{
    public interface IPaymentService
    {
        Task<IEnumerable<Domain.Payment>> GetPaymentsByBillIdAsync(Guid billId);
        Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
