namespace ProjectOversight.API.Dto
{
    public class EmployeeYesterdayTaskDetailsDTO
    {
        public decimal? TotalEstTime { get; set; }
        public decimal? TotalActTime { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
    }
}
