using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class EmployeeLeaveHistory:BaseEntity
    {
        [ForeignKey("EmployeeLeave")]
        public int? LeaveId { get; set; }
        public int? EmployeeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LeaveRequestDate { get; set; }
        public string? ApproveStatus { get; set; }
        public string? ApprovedBy { get; set; }
        public string? RejectedReason { get; set; }
        public virtual EmployeeLeave EmployeeLeave { get; set; }
    }
}
