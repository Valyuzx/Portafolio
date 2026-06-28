using System.ComponentModel.DataAnnotations;

namespace BACKEND.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string? Description { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
