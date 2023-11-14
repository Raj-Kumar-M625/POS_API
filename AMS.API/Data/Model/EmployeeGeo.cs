namespace ProjectOversight.API.Data.Model
{
    public class EmployeeGeo
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int DayId { get; set; }
        public int EmployeeTimeId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
