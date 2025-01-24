using Microsoft.EntityFrameworkCore;
using Payment.Domain;

namespace Payment.Infrastructure
{
    public class PaymentRepository:IPaymentRepository 
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pay>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Where(p => p.Bill.UserID == userId)
                .ToListAsync();
        }
    }
}