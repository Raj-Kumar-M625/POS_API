using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class PMOScrum:BaseEntity
    {
        public string EmployeeName { get; set; }
        public string? TeamName { get; set; }
        public string ScrumStatus { get; set; }
        public string? AttendanceStatus { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? ScrumTime { get; set; }
        public bool? PayAttention { get; set; }
        public int DayId { get; set; }
        [NotMapped]
        public List<PayAttention>? payAttentions { get; set; }

    }
}
