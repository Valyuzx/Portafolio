using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BACKEND.Models
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TokenId { get; set; }
        [Required]
        public Guid UserId { get; set; }
  
        [Required, MaxLength(512)]
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(45)]
        public string? CreatedByIp { get; set; }
        public DateTime? RevokedAt { get; set; }
        [MaxLength(45)]
        public string? RevokedByIp { get; set; }
        [MaxLength(512)]
        public string? ReplacedByToken { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt is not null;
        public bool IsActive => !IsRevoked && !IsExpired;
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
