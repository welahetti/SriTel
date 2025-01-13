using Billing.Domain;

namespace Billing.Service.Billing.Application.Services.Interfaces
{
    public interface IBillingService
    {
        Task<Bill> GetBillByIdAsync(int billId);
        Task<IEnumerable<Bill>> GetUserBillsAsync(int userId);
        //Task CreateBillAsync(CreateBillDTO bill);
        Task<bool> MarkBillAsPaidAsync(int billId);
    }
}
