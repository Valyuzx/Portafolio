using System.ComponentModel.DataAnnotations;

namespace BACKEND.DTOs
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        public string? Description { get; set; }
    }
}
