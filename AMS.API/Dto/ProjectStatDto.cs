namespace ProjectOversight.API.Dto
{
    public class ProjectStatDto
    {
        public int? TotalTask { get; set; }
        public int? InProgressTask { get; set; }
        public int? CompletedTask { get; set; }
        public int? TotalUserStory { get; set; }
        public int? TotalUI { get; set; }
        public int? TotalProjectObjective { get; set; }
        public string? TeamName { get; set; }
        public int? TaskPercentage { get; set; }
        public int? UserStoryPercentage { get; set; }
        public int? UIPercentage { get; set; }
        public int? ObjectivePercentage { get; set; }
        public int? InProgressPercentage { get; set; }
        public int? CompletedPercentage { get; set; }

    }
}
