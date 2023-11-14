namespace ProjectOversight.API.Dto
{
    public class TeamMemeberList
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int EmployeeId { get; set; } 
        public string? EmployeeName { get; set; }
        public decimal? ActualHour { get; set; }
        public decimal AssignedHours { get; set; }
        public decimal UnassignedHours { get; set; }
        public decimal TotalAssignedHours { get; set; }
        public decimal TotalUnassignedHours { get; set; }
        public List<SkillListDto> EmployeeSkills { get; set; }

      

    }
}
