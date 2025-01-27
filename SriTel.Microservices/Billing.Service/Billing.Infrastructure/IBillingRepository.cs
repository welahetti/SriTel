using Billing.Domain;

namespace Billing.Repositories.Interfaces
{
    public interface IBillingRepository
    {
        ////Task<Bill> GetBillAsync(Guid billId);
        Task<Bill> GetBillByBillIdAsync(Guid billId);
        Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId);
        Task AddBillAsync(Bill bill);
        Task SaveChangesAsync();      
    }
}
