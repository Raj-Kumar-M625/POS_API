namespace ProjectOversight.API.Data.Model
{
    public class TeamProjectList
    {
        public int TeamId { get; set; }
        public int Id { get; set; }
        public int projectTechStack { get; set; }
        public string TeamName { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string Status { get; set; }

    }
    public class Teamleavedetails
    {
        public string EmployeeName { get; set; }
        public DateTime? Intime { get; set; }
        public DateTime? Outtime { get; set; }
        public string TeamName { get; set; }
        public int TeamId { get; set; }
    }
}
