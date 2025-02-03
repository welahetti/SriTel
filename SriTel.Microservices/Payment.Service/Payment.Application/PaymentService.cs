using Payments.Infrastructure;
using Payments.Domain;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Payment.Service.Payment.API.DTO;

namespace Payments.Application
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly HttpClient _httpClient;

        // Inject HttpClient and IPaymentRepository
        public PaymentService(IPaymentRepository paymentRepository, IHttpClientFactory httpClientFactory)
        {
            _paymentRepository = paymentRepository;
            _httpClient = httpClientFactory.CreateClient("PaymentGateway");
        }

        // Existing method to get payments by Bill ID
        public async Task<IEnumerable<Domain.Payment>> GetPaymentsByBillIdAsync(Guid billId)
        {
            var payments = await _paymentRepository.GetPaymentsByBillIdAsync(billId);
            return payments.Select(payment => new Domain.Payment
            {
                PaymentID = payment.PaymentID,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate
            });
        }

        // Existing method to get payments by User ID
        public async Task<IEnumerable<Domain.Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);
            return payments.Select(payment => new Domain.Payment
            {
                PaymentID = payment.PaymentID,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate
            });
        }

        // New method to process payment using Mock.PaymentGateway API
        public async Task<bool> ProcessPaymentAsync(string cardNumber, decimal amount, string paymentMethod,Guid billID)
        {
            // Generate a unique TransactionId for the payment
            var transactionId = Guid.NewGuid().ToString();
            var paymentRequest = new  
            {
                BillID= billID,
                TransactionId = transactionId, // Include TransactionId
                CardNumber = cardNumber,
                Amount = amount,
                Currency = "LKR",
                PaymentMethod = paymentMethod // Pass the payment method
            };

            var content = new StringContent(JsonSerializer.Serialize(paymentRequest), System.Text.Encoding.UTF8, "application/json");

            // Call Mock.PaymentGateway API to process payment
            var response = await _httpClient.PostAsync("/api/PaymentGateway/payments", content);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response content
                var responseContent = await response.Content.ReadAsStringAsync();
                var paymentResponse = JsonSerializer.Deserialize<ProcessPaymentResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (paymentResponse != null)
                {
                    // Extract the last four digits of the card number
                    var lastFourDigits = cardNumber.Length >= 4
                        ? cardNumber.Substring(cardNumber.Length - 4)
                        : cardNumber;

                    // Detect card type
                    var cardType = GetCardType(cardNumber);

                    // Save payment details to the database
                    var payment = new Domain.Payment
                    {
                        BillID =billID ,
                        PaymentID = Guid.NewGuid(),
                        AmountPaid = amount,
                        PaymentDate = DateTime.UtcNow,
                        CardLastFourDigits = lastFourDigits, // Save last four digits
                        CardType = cardType, // Save card type
                        TransactionID = paymentResponse.TransactionId,
                        PaymentMethod = paymentMethod, // Save payment method
                    };

                    await _paymentRepository.AddPaymentAsync(payment);
                    return true; // Payment processed successfully
                }
            }

            return false; // Payment failed
        }

        private string GetCardType(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return "Unknown";

            // Use regex patterns to identify card types
            if (System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$"))
                return "Visa";
            if (System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^5[1-5][0-9]{14}$"))
                return "MasterCard";
            if (System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^3[47][0-9]{13}$"))
                return "American Express";
            if (System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^6(?:011|5[0-9]{2})[0-9]{12}$"))
                return "Discover";

            return "Unknown"; // Default to Unknown if no patterns match
        }


        // New method to check payment status using Mock.PaymentGateway API
        public async Task<string> CheckPaymentStatusAsync(string transactionId)
        {
            // Call Mock.PaymentGateway API to check payment status
            var response = await _httpClient.GetAsync($"/api/payment/status/{transactionId}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody; // Return payment status
            }

            return "Failed to retrieve payment status";
        }
    }
}