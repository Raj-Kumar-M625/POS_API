namespace ProjectOversight.API.Dto
{
    public class TaskCheckListDto
    {
        public int UIUserStoryId { get; set; }
        public int ProjectId { get; set; }
        public int CategoryId { get; set; }
        public int UIId { get; set; }
        public int UserStoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int Percentage { get; set; }
        public DateTime EstimateStartDate { get; set; }
        public DateTime EstimateEndDate { get; set; }
        public string TaskType { get; set; }
        public string Classification { get; set; }
        public string Comment { get; set; }
        public int EmployeeId { get; set; }
        public string? TaskDescription { get; set; }
        public string[] CheckListDescriptions { get; set; }
    }
}
