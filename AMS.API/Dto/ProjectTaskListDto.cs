using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class ProjectTaskListDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Names { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int percentage { get; set; }
        public decimal estTime { get; set; }
        public decimal actTime { get; set; }
        public DateTime EstimatedStartedDate { get; set; }
        public DateTime EstimatedEndedDate { get; set; }

    }
}
