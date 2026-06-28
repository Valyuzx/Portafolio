using System.ComponentModel.DataAnnotations.Schema;

namespace BACKEND.Models
{
    public class ProjectTechnology
    {
        public Guid ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; } = null!;
        public Guid TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public Technology Technology { get; set; } = null!;
    }
}
