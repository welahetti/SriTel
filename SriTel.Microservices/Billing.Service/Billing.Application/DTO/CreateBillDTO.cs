using System.ComponentModel.DataAnnotations;

namespace Billing.Application.DTO
{
    public class CreateBillDTO
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
