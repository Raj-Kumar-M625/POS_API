using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public int? LeadId { get; set; }
        public string Username { get; set; }
        public string? EmployeeName { get; set; }
        public decimal? EstHour { get; set; }
        public decimal? ActualHour { get; set; }
        public int? ProjectId { get; set; }

    }

    public class TeamVM
    {
        public string Name { get; set; }
        public int TeamId { get; set; }
        public decimal AssignedHours { get; set; }
        public decimal UnAssignedHours { get; set; }
    }

    public class TeamProjectDto
    {
        public int TeamId { get; set; }
        public int id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int? Percentage { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
    }

}
