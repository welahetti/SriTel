using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Customers.Domain
{
    public class Bill
    {
        [Key]
        public Guid BillID { get; set; }

        [ForeignKey("User")]
        public Guid UserID { get; set; }

        [Required]
        public decimal Amount { get; set; }  // Total bill amount

        [Required]
        public DateTime DueDate { get; set; }  // Due date for payment

        [Required]
        public DateTime BillingDate { get; set; }  // The date the bill was generated

        public bool IsPaid { get; set; }  // Whether the bill has been paid 

        // Navigation property (logical, not persisted in DB)
        [NotMapped]
        public Domain.Customer customer { get; set; }

    }
}
