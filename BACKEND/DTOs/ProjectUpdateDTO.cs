using System.ComponentModel.DataAnnotations;

namespace BACKEND.DTOs
{
    public class ProjectUpdateDTO
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El título no puede exceder 150 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El resumen es obligatorio.")]
        [MaxLength(300, ErrorMessage = "El resumen no puede exceder 300 caracteres.")]
        public string Resume { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(300, ErrorMessage = "La descripción no puede exceder 300 caracteres.")]
        public string Description { get; set; } = string.Empty;
        public string? RepositoryURL { get; set; }
        public string? DemoUrl { get; set; }

        [Required(ErrorMessage = "La imagen principal es obligatoria.")]
        public string PrincipalImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de desarrollo es obligatoria.")]
        public DateTime DevelopmentDate { get; set; }

        public bool IsPublished { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public Guid CategoryId { get; set; }

        public List<Guid> TechnologyIds { get; set; } = [];
    }
}
