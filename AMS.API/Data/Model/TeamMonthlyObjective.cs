using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class TeamMonthlyObjective : BaseEntity
    {
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string? Status { get; set; }
        public int? Percentage { get; set; }
        [NotMapped]
        public string? ProjectName { get; set; }
        [NotMapped]
        public List<TeamWeeklyObjective>? TeamWeeklyObjectives { get; set; }
    }
}
