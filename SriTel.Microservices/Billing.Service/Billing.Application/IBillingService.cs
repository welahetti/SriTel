﻿using Billing.Domain;

namespace SriTel.Billing.Application.Services.Interfaces
{
    public interface IBillingService
    {
        Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId);
        Task<Bill> GetBillByBillIdAsync(Guid billId);

        Task CreateBillAsync(Bill bill);
    }
}
