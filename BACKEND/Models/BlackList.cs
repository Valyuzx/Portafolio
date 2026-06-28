using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BACKEND.Models
{
    public class BlackList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TokenId { get; set; } = null!;
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
