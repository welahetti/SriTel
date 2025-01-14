using Billing.Domain;

namespace SriTel.Billing.Application.Services.Interfaces
{
    public interface IBillingService
    {
        Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId);
    }
}
