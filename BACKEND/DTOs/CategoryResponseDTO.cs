namespace BACKEND.DTOs
{
    public class CategoryResponseDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
