namespace ProjectOversight.API.Dto
{
    public class LeaveHistoryDto
    {
        public int Id { get; set; }
        public int? LeaveId { get; set; }
        public string? LeaveType { get; set; }
        public string? LeaveSubType { get; set; }
        public int EmployeeId { get; set; }
        public int? TeamId { get; set; }
        public int? LeaveStatusCount { get; set; }
        public DateTime? LeaveRequestDate { get; set; }
        public string? ApproveStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? LeaveReason { get; set; }    
        public DateTime? UpdatedDate { get; set; }
        public string?UpdatedBy { get; set; }
        public string? EmployeeName { get; set; }

    }
}
