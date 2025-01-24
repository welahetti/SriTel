using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payments.Domain
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; }

        [ForeignKey("Bill")]
        public Guid BillID { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } // e.g., CreditCard, DebitCard, Wallet

        [MaxLength(100)]
        public string TransactionID { get; set; } // Gateway transaction ID

        public bool IsSuccess { get; set; }

        // Optional: Non-sensitive card details for reference
        [MaxLength(4)]
        public string CardLastFourDigits { get; set; } // e.g., 1234

        [MaxLength(20)]
        public string CardType { get; set; } // e.g., VISA, MasterCard

        // Navigation property
        public Bill Bill { get; set; }
    }
}
