namespace ProjectOversight.API.Data.Model
{
    public class TeamWeeklyProjectItem
    {
        public int TeamId { get; set; }
        public string ProjectName { get; set; }
        public decimal AssignedHours { get; set; }
    }
}
