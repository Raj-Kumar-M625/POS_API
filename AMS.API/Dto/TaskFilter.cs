namespace ProjectOversight.API.Dto
{
    public class TaskFilter
    {
        public string? Name { get; set; }
        public string? ProjectName { get; set; }
        public string? Status { get; set; }
        public double? Percentage { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public decimal? ActualTime { get; set; }
        public decimal? EstimatedTime { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public DateTime? ActStartDate { get; set; }
        public DateTime? ActEndDate { get; set; }
        public string? EmployeeName { get; set; }
        public string? TaskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Priority { get; set; }
        public DateTime? WeekEndingDate { get; set; }
        public string? ProjectType { get; set; }
        public string? TeamName { get; set; }
    }
}
