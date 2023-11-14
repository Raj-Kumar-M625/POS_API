namespace ProjectOversight.API.Data.Model
{
    public class EmployeeLeave:BaseEntity
    {
        public int? TeamId { get; set; }
        public int? EmployeeId { get; set; }
        public string? LeaveType { get; set; }
        public string? LeaveSubType { get; set; }
        public string? LeaveStatus { get; set; } 
        public string? LeaveReason { get; set; }
       
    }
}
