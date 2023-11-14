namespace ProjectOversight.API.Data.Model
{
    public class ProjectCheckList
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public int DevCheckCount { get; set; }
        public int QACheckCount { get; set; }
        public string  TaskName { get; set; }
        public string  UserName { get; set; }
        public List<UserTaskCheckList>  userTaskCheckLists { get; set;}  
    }
}
