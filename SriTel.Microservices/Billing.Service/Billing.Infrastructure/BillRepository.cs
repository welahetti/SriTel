using Billing.Domain;
using Billing.Service.Billing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SriTel.Billing.Repositories.Interfaces;

namespace SriTel.Repositories.Implementations
{
    public class BillRepository(BillingDbContext context) : IBillingRepository
    {
        private readonly BillingDbContext _context = context;

        public async Task<Bill> GetBillAsync(Guid billId)
        {
            return await _context.Bills.Include(b => b.Payments).FirstOrDefaultAsync(b => b.BillID == billId);
        }

        public async Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId)
        {
            return await _context.Bills.Where(b => b.UserID == userId).ToListAsync();
        }

        public async Task AddBillAsync(Bill bill)
        {
            await _context.Bills.AddAsync(bill);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bill>> GetBillsByBillIdAsync(Guid billId)
        {
            return await _context.Bills.Where(p => p.BillID == billId).ToListAsync();
        }

        public Task<IEnumerable<Bill>> GetBillsByBillIdAsync(int billId)
        {
            throw new NotImplementedException();
        }
    }
}