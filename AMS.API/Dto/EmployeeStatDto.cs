namespace ProjectOversight.API.Dto
{
    public class EmployeeStatDto
    {
        public int TotalProject { get; set; }
        public int TotalTask { get; set; }
        public int InProgressTask { get; set; }
        public int CompletedTask { get; set; }
    }
}
