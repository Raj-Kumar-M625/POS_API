namespace ProjectOversight.API.Dto
{
    public class AttendenceFilterDto
    {
        public int? employeeId { get; set; }
        public int? projectId { get; set; }

        public List<int>? months { get; set; }

        public List<string>? status { get; set; }
        public List<string>? weekendingDate { get; set; }
    }
}
