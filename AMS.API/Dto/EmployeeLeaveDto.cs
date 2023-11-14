using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class EmployeeLeaveDto
    { 
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public string? LeaveType { get; set; }
        public string? LeaveSubType { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<DateTime>? LeaveRequestDate { get; set; }
        public string? LeaveStatus { get; set; }
        public string? LeaveReason { get; set; }
        public List<EmployeeLeaveHistory>? LeaveHistory { get; set; }
        public string? EmployeeName { get; set; }
       
      

    }
}
