using Payments.Domain;

namespace Payments.Infrastructure
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
