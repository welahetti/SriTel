﻿using Billing.Domain;
using Billing.Service.Billing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Billing.Repositories.Interfaces;
using Billing.Infrastructure;

namespace Billing.Repositories.Implementations
{
    public class BillingRepository: IBillingRepository
    {
        private readonly BillingDbContext _context;
        public BillingRepository(BillingDbContext context)
        {
            _context = context;
        }

        //public async Task<Bill> GetBillAsync(Guid billId)
        //{
        //    return await _context.Bills.Include(b => b.Payments).FirstOrDefaultAsync(b => b.BillID == billId);
        //}

        public async Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId)
        {
            return await _context.Bills.Where(b => b.UserID == userId).ToListAsync();
        }

        public async Task AddBillAsync(Bill bill)
        {
            _context.Bills.AddAsync(bill);
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Bill> GetBillByBillIdAsync(Guid billId)
        {
            return await _context.Bills.FirstOrDefaultAsync(b => b.BillID == billId);
        }
    }
}