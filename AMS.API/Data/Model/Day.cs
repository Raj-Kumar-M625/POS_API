namespace ProjectOversight.API.Data.Model
{
    public class Day
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public  DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? HolidayName { get; set; }
        public bool? HolidayApplicable { get; set; }
    }
}
