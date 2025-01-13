using Billing.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Billing.Service.Billing.Domain
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; }

        [ForeignKey("Bill")]
        public Guid BillID { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }  // Amount paid by the user

        [Required]
        public DateTime PaymentDate { get; set; }  // The date the payment was made

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }  // E.g., Credit Card, Debit Card, etc.

        [MaxLength(100)]
        public string TransactionID { get; set; }  // External transaction ID from the payment gateway

        public bool IsSuccess { get; set; }  // Whether the payment was successful


        // Navigation property
        [ForeignKey("BillID")]
        public Bill Bill { get; set; }
    }
}
