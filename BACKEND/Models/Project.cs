using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BACKEND.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectId { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Resume { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public string? RepositoryURL { get; set; }
        public string? DemoUrl { get; set; }
        [Required]
        public string PrincipalImageUrl { get; set; } = string.Empty;
        public DateTime DevelopmentDate { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;
        public ICollection<ProjectTechnology> ProyectoTecnologys { get; set; } = new List<ProjectTechnology>();
    }
}
