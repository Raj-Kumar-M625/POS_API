using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Data.Model
{
    public class ProjectTask : BaseEntity
    {
        public int TotalTask { get; set; }
        public int OnGoing { get; set; }  
        public int Completed { get; set; }
        public int Assigned { get; set; }
        public int ReadyForUAT { get; set; }
        public int Unassigned { get; set; }
        public List<WeeklyTaskList> WeekTask { get; set; }
        public List<MonthlyTaskList> monthlyTaskLists { get; set; }
        public List<ProjectTaskListDto> ProjectTaskLists { get; set; } 


    }
}
