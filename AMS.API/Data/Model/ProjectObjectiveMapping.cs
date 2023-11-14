namespace ProjectOversight.API.Data.Model
{
    public class ProjectObjectiveMapping : BaseEntity
    {
        public int ProjectObjectiveId { get; set; }
        public int UserStoryId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
    }
}
