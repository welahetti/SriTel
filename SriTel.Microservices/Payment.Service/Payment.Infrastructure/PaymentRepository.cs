using Microsoft.EntityFrameworkCore;
using Payments.Domain;

namespace Payments.Infrastructure
{
    public class PaymentRepository:IPaymentRepository 
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBillIdAsync(Guid billId)
        {
            return await _context.Payments
              .Where(p => p.Bill.BillID == billId)
              .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Where(p => p.Bill.UserID == userId)
                .ToListAsync();
        }
    }
}