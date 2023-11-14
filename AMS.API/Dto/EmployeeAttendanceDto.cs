using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class EmployeeAttendanceDto
    {
        public DateTime? Date { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public List<EmployeeGeo> EmployeeGeo { get; set; }
    }

    public class EmployeeAttendanceVM
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int TotalAttendance { get; set; }
        public int TotalAbsent { get; set; }
        public string? AverageInTime { get; set; }
        public string? AverageOutTime { get; set; }
        public List<EmployeeAttendanceDto> EmployeeAttendances { get; set; }
    }
}
