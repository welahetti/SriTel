using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notification.Domain
{
    
        [Table("FCM_Tokens")]
        public class FcmToken
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            public int UserId { get; set; }

            [Required]
            [MaxLength] // NVARCHAR(MAX)
            public string Token { get; set; }
        }
    }
