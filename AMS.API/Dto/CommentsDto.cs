using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Dto
{
    public class CommentsDto
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public string? Project { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        [ForeignKey("Task")]
        public string? Employee { get; set; }
        public int? TaskId { get; set; }
        [ForeignKey("EmployeeTask")]
        public int? EmployeeTaskId { get; set; }
        [ForeignKey("EmployeeDailyTask")]
        public int? EmployeeDailyTaskId { get; set; }
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public int? EmployeeTimeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Status { get; set; }
        public int Percentage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
