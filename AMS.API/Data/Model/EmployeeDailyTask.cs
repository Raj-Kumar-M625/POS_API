using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class EmployeeDailyTask :BaseEntity
    {
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeTask")]
        public int EmployeeTaskId { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectObjective")]
        public int? ProjectObjectiveId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int Percentage { get; set; }
        public DateTime? WorkedOn { get; set; }
        public virtual EmployeeTask EmployeeTask { get; set; }
    }
}
