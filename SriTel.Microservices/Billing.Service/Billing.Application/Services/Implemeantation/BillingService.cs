using Billing.Domain;
using Billing.Service.Billing.Application.ExternalServices;
using SriTel.Billing.Application.Services.Interfaces;
using SriTel.Billing.Repositories.Implementations;
using SriTel.Billing.Repositories.Interfaces;

namespace SriTel.Billing.Application.Services
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _billRepository;

        public BillingService(IBillingRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public async Task<Bill> GetBillByIdAsync(Guid billId)
        {
            var bill = await _billRepository.GetBillAsync(billId);
            if (bill == null) throw new Exception("Bill not found");

            // Map to DTO
            return new Bill
            {
                BillID = bill.BillID,
                Amount = bill.Amount,
                DueDate = bill.DueDate,
                IsPaid = bill.IsPaid
            };
        }

        public async Task<IEnumerable<Bill>> GetBillsByUserAsync(Guid userId)
        {
            var bills = await _billRepository.GetBillsByUserAsync(userId);

            return bills.Select(b => new Bill
            {
                BillID = b.BillID,
                Amount = b.Amount,
                DueDate = b.DueDate,
                IsPaid = b.IsPaid
            });
        }

        //public async Task CreateBillAsync(CreateBillDTO billDto)
        //{
        //    var bill = new Bill
        //    {
        //        UserID = billDto.UserID,
        //        Amount = billDto.Amount,
        //        DueDate = billDto.DueDate,
        //        IsPaid = false
        //    };

        //    await _billRepository.AddAsync(bill);
        //}

        public async Task<bool> MarkBillAsPaidAsync(Guid billId)
        {
            var bill = await _billRepository.GetBillAsync(billId);
            if (bill == null) throw new Exception("Bill not found");

            bill.IsPaid = true;
            await _billRepository.SaveChangesAsync();

            return true;
        }

        public Task<bool> MarkBillAsPaidAsync(int billId)
        {
            throw new NotImplementedException();
        }

      
    }
}
