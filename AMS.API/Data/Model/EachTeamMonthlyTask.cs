using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Data.Model
{
    public class EachTeamMonthlyTask
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int EmployeeId { get; set; }

        public int Month { get; set; }
        public decimal ActualHour { get; set; }

        public decimal EstHour { get; set; }
    }
}
