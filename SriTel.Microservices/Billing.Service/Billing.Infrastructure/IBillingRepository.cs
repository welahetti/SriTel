using Billing.Domain;

namespace SriTel.Billing.Repositories.Interfaces
{
    public interface IBillingRepository
    {
        Task<Bill> GetBillAsync(Guid billId);
        Task<IEnumerable<Bill>> GetBillsByBillIdAsync(Guid billId);
        Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId);
        Task AddBillAsync(Bill bill);
        Task SaveChangesAsync();      
    }
}
