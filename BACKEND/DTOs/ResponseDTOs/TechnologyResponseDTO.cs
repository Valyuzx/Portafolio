namespace BACKEND.DTOs.ResponseDTOs
{
    public class TechnologyResponseDTO
    {
        public Guid TechnologyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconUrl { get; set; }
    }
}
