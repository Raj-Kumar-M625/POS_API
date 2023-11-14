using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class UserStory :BaseEntity
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectObjective")]
        public int? ProjectObjectiveId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }                                                                                                                     
        public string Status { get; set; }
        public int Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public List<UserStoryUIDto>? userStoryUIs { get; set; }
        [NotMapped]
        public int[] ProjectObjectiveIds { get; set; }
        [NotMapped]
        public List<Document>? Documents { get; set; }
    }
}
