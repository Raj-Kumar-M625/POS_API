using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class TeamEmployee : BaseEntity
    {
        [ForeignKey("Team")]
        public int TeamId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [ForeignKey("Lead")]
        public int? LeadId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }   
        public virtual Employee? Employee { get; set; }          
        public virtual Team? Team { get; set; }          
    }

    public class TeamEmployeeDto
    {
        public int EmployeeId { get; set; }
        public int TeamId { get; set; }
        public string EmployeeName { get; set; }
        public string TeamName { get; set; }

        public decimal? AssignedHours { get; set; }
        public decimal? UnassignedHours { get; set; }
        //public object TeamName { get; internal set; }
    }
}
