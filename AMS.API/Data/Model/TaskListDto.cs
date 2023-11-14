namespace ProjectOversight.API.Data.Model
{
    public class TaskListDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? EmployeeTaskId { get; set; }
        public string Name { get; set; }
        public string projectName { get; set; }
        public string Description { get; set; }
        public string TeamName { get; set; }
        public string EmployeeName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public int Percentage { get; set; }
        public decimal EstimateTime { get; set; }
        public decimal ActualTime { get; set; }
        public int? TeamId { get; set; }
        public string USName { get; set; }
        public string UIName{ get; set; }
        public DateTime? EstimateStartDate { get; set; }
        public DateTime? EstimateEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? weekEndDate { get; set; }
        public int? UserStoryUIId { get; set; }
        public int? UserStoryId { get; set;}
        public string TaskType { get; set; }
        public string? Priority { get; set; }
    }
}
