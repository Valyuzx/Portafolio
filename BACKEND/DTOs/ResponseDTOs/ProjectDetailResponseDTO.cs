namespace BACKEND.DTOs.ResponseDTOs
{
    public class ProjectDetailResponseDTO
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Resume { get; set; } = string.Empty;
        public string Description { get; set; } =
            string.Empty;
        public string? RepositoryURL { get; set; }
        public string? DemoUrl { get; set; }
        public string PrincipalImageUrl { get; set; } =
            string.Empty;
        public DateTime DevelopmentDate { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } =
            string.Empty;
        public List<TechnologyItemDTO> Technologies
            { get; set; } = [];
    }

    public class TechnologyItemDTO
    {
        public Guid TechnologyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconUrl { get; set; }
    }
}
