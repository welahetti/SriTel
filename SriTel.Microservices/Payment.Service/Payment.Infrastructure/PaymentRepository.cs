using Microsoft.EntityFrameworkCore;
using Payments.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        // Get payments by Bill ID
        public async Task<IEnumerable<Domain.Payment>> GetPaymentsByBillIdAsync(Guid billId)
        {
            var payments = await _context.Payments
                .Where(p => p.Bill.BillID == billId)
                .Select(p => new Domain.Payment
                {
                    PaymentID = p.PaymentID,
                    AmountPaid = p.AmountPaid,
                    PaymentDate = p.PaymentDate,
                })
                .ToListAsync();

            return payments;
        }

        // Get payments by User ID
        public async Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            var payments = await _context.Payments
                .Where(p => p.Bill.UserID == userId)
                .Select(p => new Domain.Payment
                {
                    PaymentID = p.PaymentID,
                    AmountPaid = p.AmountPaid,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync();

            return payments;
        }

        // Add a new payment to the database
        public async Task AddPaymentAsync(Domain.Payment payment)
        {
            var infrastructurePayment = new Domain.Payment
            {
                PaymentID = payment.PaymentID,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate,
                BillID = payment.BillID,
                CardLastFourDigits =payment.CardLastFourDigits ,
                TransactionID=payment.TransactionID,
                CardType =payment.CardType ,
                PaymentMethod =payment.PaymentMethod // Ensure this property exists in Domain.Payment
               
                
            };

            _context.Payments.Add(infrastructurePayment);
            await _context.SaveChangesAsync();
        }
    }
}