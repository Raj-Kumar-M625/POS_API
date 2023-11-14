namespace ProjectOversight.API.Data.Model
{
    public class TaskCheckList:BaseEntity
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public int USId { get; set; }
        public int UIId { get; set; }
        public int CategoryId { get; set; }
        public string CheckListDescription { get; set; }
        public bool IsDevChecked { get; set; }
        public bool IsQAChecked { get; set; }
        public int? UserStoryUIId { get; set; }
        public List<UserTaskCheckList> userTaskCheckList { get; set; }
    }
}
