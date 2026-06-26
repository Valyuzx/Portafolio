using System.ComponentModel.DataAnnotations;

namespace BACKEND.Models
{
    public class RefreshToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
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
    }
}
