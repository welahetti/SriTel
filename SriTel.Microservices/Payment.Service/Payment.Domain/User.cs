using System.ComponentModel.DataAnnotations;

namespace Payment.Domain
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }  // Stored securely (hashed)

        [MaxLength(255)]
        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; }  // Whether the account is active or disabled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public ICollection<Bill> Bills { get; set; }  // Relationship with Bill
    }
}
