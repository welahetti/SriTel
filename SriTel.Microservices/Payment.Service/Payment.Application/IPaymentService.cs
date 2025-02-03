
using Payments.Domain;

namespace Payments.Application
{
    public interface IPaymentService
    {
        Task<IEnumerable<Domain.Payment>> GetPaymentsByBillIdAsync(Guid billId);
        Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId);
        Task<bool> ProcessPaymentAsync(string cardNumber, decimal amount, string paymentMethod, Guid billID);
        Task<string> CheckPaymentStatusAsync(string transactionId);
    }
}
