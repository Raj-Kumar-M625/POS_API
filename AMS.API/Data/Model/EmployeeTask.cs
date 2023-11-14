using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class EmployeeTask :BaseEntity
    {
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectObjective")]
        public int? ProjectObjectiveId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int Percentage { get; set; }

        [ForeignKey(nameof(Day))]
        public int? DayId { get; set; }
        public DateTime EstStartDate { get; set; }
        public DateTime EstEndDate { get; set; }

       // public virtual Project Project { get; set; }
      //public virtual Task Task { get; set; }        
       public virtual TeamEmployee Employee { get; set; }

    }
}
