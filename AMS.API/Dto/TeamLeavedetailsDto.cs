using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class TeamLeavedetailsDto
    {
        public List<EmployeeTime> EmployeeTimes { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string? EmployeeName { get; set; }
        public int AbsenceCount { get; set; }  

    }
    public class TeamLeaveVM
    {
        public List<Day> Days { get; set; }
        public List<TeamLeavedetailsDto> TeamLeaveDetails { get; set; }

    }
}