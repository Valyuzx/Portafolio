namespace BACKEND.DTOs.ResponseDTOs
{
    public class ProjectResponseDTO
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Resume { get; set; } = string.Empty;
        public string PrincipalImageUrl { get; set; } =
            string.Empty;
        public DateTime DevelopmentDate { get; set; }
        public bool IsPublished { get; set; }
        public string CategoryName { get; set; } =
            string.Empty;
        public List<string> Technologies { get; set; } = [];
    }
}
