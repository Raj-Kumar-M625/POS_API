namespace ProjectOversight.API.Dto
{
    public class MonthlyLists
    {
        public int month { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
        public int totalCompleted { get; set; }
        public int totalInProgress { get; set; }
        public int totalReadyForUAT { get; set; }
    }
}
