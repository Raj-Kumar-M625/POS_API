namespace ProjectOversight.API.Dto
{
    public class MonthlyHours
    {
        public int month { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
    }
}
