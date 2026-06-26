using System.ComponentModel.DataAnnotations;

namespace BACKEND.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;  // "Admin", "Owner"
        [MaxLength(255)]
        public string? Description { get; set; }
        // Navegación
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
