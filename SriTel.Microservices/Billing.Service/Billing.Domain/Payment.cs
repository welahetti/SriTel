using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Billing.Domain
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
        public string PaymentMethod { get; set; }

        [MaxLength(100)]
        public string TransactionID { get; set; }

        public bool IsSuccess { get; set; }

        // Navigation property
        public Bill Bill { get; set; }
    }
}