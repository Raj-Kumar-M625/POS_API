namespace ProjectOversight.API.Data.Model
{
    public class EmployeeWorkFlow
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? WorkFlowId { get; set; }
        public int? DayId { get; set; }
    }
}
