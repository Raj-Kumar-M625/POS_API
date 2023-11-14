namespace ProjectOversight.API.Dto
{
    public class ProjectCatergory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalDevelopment { get; set; }
        public int TotalBusinessAnalyst { get; set; }
        public int TotalTesting { get; set; }   
    }
}
