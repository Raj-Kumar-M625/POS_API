using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class Project: BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        [Range(0, 100)]
        public int? Percentage { get; set; }
        public virtual ProjectDocuments? ProjectDocuments { get; set; }
        public virtual ICollection<ProjectTechStack>? ProjectTechStacks { get; set; }
        [NotMapped]
        public int[]? TechStackId { get; set; }
        [NotMapped]
        public int TeamId { get; set; }


    }
}
