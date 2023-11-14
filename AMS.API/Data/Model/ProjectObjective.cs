using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class ProjectObjective : BaseEntity
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Percentage { get; set; }

    }

}
