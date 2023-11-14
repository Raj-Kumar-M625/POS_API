using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class UserInterface :BaseEntity
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectObjective")]
        public int? ProjectObjectiveId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Complexity { get; set; }
        public string? UICategory { get; set; }
        public int Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public List<Document>? Documents { get; set; }
    }

}
