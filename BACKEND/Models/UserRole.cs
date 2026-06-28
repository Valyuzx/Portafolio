using System.ComponentModel.DataAnnotations.Schema;

namespace BACKEND.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
