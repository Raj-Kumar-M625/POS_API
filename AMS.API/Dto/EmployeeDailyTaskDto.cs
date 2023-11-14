using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class EmployeeDailyTaskDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? TaskId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ProjectName { get; set; }
        public int EmployeeTaskId { get; set; }
        public int ProjectId { get; set; }
        public int? ProjectObjectiveId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal? ActTime { get; set; }
        public DateTime? WeekEndingDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int? Percentage { get; set; }
        public string Name { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public DateTime? WorkedOn { get; set; }
        public EmployeeTaskDto? EmployeeTask { get; set; }
        //public virtual TaskCheckList TaskCheckList { get; set; }
        public int[]? TaskChecklistId { get; set; }
    }
}
