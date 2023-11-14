namespace ProjectOversight.API.Data.Model
{
    public class TaskReleaseNotes:BaseEntity
    {
        public int? ReleaseNotesId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
        public string? Version { get; set; }
    }
}
