namespace BACKEND.DTOs.ResponseDTOs
{
    public class CategoryResponseDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
