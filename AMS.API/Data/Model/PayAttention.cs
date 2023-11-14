namespace ProjectOversight.API.Data.Model
{
    public class PayAttention:BaseEntity
    {
        public int EmployeeId { get; set; }
        public int DayId { get; set; }
        public string Reason { get; set; }
        public int PMOScrumId { get; set; }  
  
    }
}
