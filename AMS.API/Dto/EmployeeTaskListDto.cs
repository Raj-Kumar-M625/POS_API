using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Dto
{
    public class EmployeeTaskListDto : BaseEntity
    {
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Percentage { get; set; }
        public DateTime EstStartDate { get; set; }
        public DateTime EstEndDate { get; set; }
        public string Comment { get; set; }
    }
}
