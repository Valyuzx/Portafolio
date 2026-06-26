using System.ComponentModel.DataAnnotations;

namespace BACKEND.Models
{
    public class Technology
    {
        [Key]
        public int TechnologyId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? IconUrl { get; set; }

        public ICollection<ProjectTechnology> ProyectoTechnologies { get; set; } = new List<ProjectTechnology>();
    }
}
