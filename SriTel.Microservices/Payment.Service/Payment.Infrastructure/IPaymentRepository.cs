using Payment.Domain;

namespace Payment.Infrastructure
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Pay>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
