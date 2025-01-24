
using Payment.Domain;

namespace Payment.Application
{
    public interface IPaymentService
    {
        Task<IEnumerable<Pay>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
