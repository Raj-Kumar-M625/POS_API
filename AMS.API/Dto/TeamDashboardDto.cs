using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Dto
{
    public class TeamDashboardDto
    {
        public int totalTeamEmployees { get; set; }
        public int TotalTeamProject { get; set; }
        public int totalteamObjectives { get; set; }
        public int TotalWeekObjective { get; set; }
        public int TotalMonthObjective { get; set; }
        public List<ProjectTechStackDto> ProjectTechstack { get; set; }

        public List<TeamMemeberList> TeamMember { get; set; }
        public List<TeamDashboardListDto> TeamDashboardListDtos { get; set; }

    }
}
