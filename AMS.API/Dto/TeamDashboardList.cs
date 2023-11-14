using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Dto
{
    public class TeamDashboardListDto

    {
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public string Team { get; set; }
        public string MonthlyObjcectiveName { get; set; } 
        public string ObjectiveStatus { get; set; }  
        public string Objective { get; set; }   
        public string WeeklyObjectives { get; set; }
        public string WeeklyObjectiveDescription { get; set; }
        public string? ProjectName { get; set; }
        public int? Percentage { get; set; } 
        public string Type { get; set; }
        public int Id { get; set; }
    }

}
