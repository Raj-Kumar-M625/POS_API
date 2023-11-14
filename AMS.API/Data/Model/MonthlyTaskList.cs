namespace ProjectOversight.API.Data.Model
{
    public class MonthlyTaskList
    {
        public int Id { get; set; }
        public int EmoloyeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ProjectId { get; set; }  
        public string Names { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int percentage { get; set; }
        public DateTime WeekEndingDate { get; set; } 
        public decimal actualTime { get; set; }
        public decimal EstimedTime { get; set; }
        public DateTime EstimatedStartedDate { get; set; }
        public DateTime EstimatedEndedDate { get; set; }
        
    }
}
