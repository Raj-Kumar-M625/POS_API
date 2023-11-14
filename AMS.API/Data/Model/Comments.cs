using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class Comments :BaseEntity
    {
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        [ForeignKey("Task")]
        public int? TaskId { get; set; }
        [ForeignKey("EmployeeTask")]
        public int? EmployeeTaskId { get; set; }
        [ForeignKey("EmployeeDailyTask")]
        public int? EmployeeDailyTaskId { get; set; }
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public int? EmployeeTimeId { get; set; }
        public string Comment{ get; set; }
    }
}
