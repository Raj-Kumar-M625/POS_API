namespace ProjectOversight.API.Dto
{
    public class TeamAttendenceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public DateTime? Date { get; set; }
        public string? Comments { get; set; }
        
    }

    public class TeamAttendanceStatDto
    {
        public string AvgInTime { get; set; }
        public string AvgOutTime { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public List<TeamAttendenceDto> TeamAttendanceData { get; set; }
    }
}
