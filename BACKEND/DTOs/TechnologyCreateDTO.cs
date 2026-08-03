using System.ComponentModel.DataAnnotations;

namespace BACKEND.DTOs
{
    public class TechnologyCreateDTO
    {
        [Required(ErrorMessage = "El nombre de la tecnología es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(255, ErrorMessage = "La URL del icono no puede exceder 255 caracteres.")]
        public string? IconUrl { get; set; }
    }
}
