using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class EmployeeTime : BaseEntity
    {
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public int? DayId { get; set; }
    }
}
