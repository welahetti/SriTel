
using Payments.Domain;

namespace Payments.Application
{
    public interface IPaymentService
    {
        Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId);
    }
}
