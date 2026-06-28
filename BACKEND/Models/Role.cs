using System.ComponentModel.DataAnnotations;

namespace BACKEND.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;  // "Admin", "Owner"
        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public short Order { get; set; } = 0;
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
