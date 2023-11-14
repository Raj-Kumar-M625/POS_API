namespace ProjectOversight.API.Dto
{
    public class TaskDTO
    {
        public int? UIUserStoryId { get; set; }
        public int Id { get; set; }
        public int EmployeeTaskId { get; set; }
        public int ProjectId { get; set; }
        public int TeamId { get; set; }
        public int CategoryId { get; set; }   
        public int UIId { get; set; }
        public int? UserStoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal? ActTime { get; set; }
        public DateTime? WeekEndingDate { get; set; }
        public string Status { get; set; }
        public string? Priority { get; set; }
        public int Percentage { get; set; }
        public DateTime EstimateStartDate { get; set; }
        public DateTime EstimateEndDate { get; set; }
        public string? TaskType { get; set; }
        public string? Classification { get; set; }
        public string? Comment { get; set; }
        public List<string>? Comments   { get; set; }
        public int EmployeeId { get; set; }
        public string? TaskDescription { get; set; }
        public int? UserStoryUIId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? EmployeeName { get; set; }
        public string UpdatedBy { get; set; }
        public int? CompletedTaskCount { get; set; }
        public int? InProgressTaskCount { get; set; }
    }
}
