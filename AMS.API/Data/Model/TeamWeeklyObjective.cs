using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class TeamWeeklyObjective : BaseEntity
    {
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public int? MonthlyObjectiveId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string? Status { get; set; }
        public int? Percentage { get; set; }
        public DateTime? WeekEndingDate { get; set; }
        [NotMapped]
        public string? ProjectName { get; set; }


    }
}
