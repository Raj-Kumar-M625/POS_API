using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class ProjectDashboardDto
    {
        public CommonUserStory CommonUserStory { get; set; }
        public CommonUserInterface CommonUserInterface { get; set; }
        public CommonTask CommonTask { get; set; }
        public CommonProject CommonProject { get; set; }
        public List<Project> Projects { get; set; }
        public List<UserStory> UserStories { get; set; }
        public List<UserInterface> UserInterface { get; set; }
    }

    public class CommonUserStory
    {
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int NotStarted { get; set; }
    }

    public class CommonUserInterface
    {
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int NotStarted { get; set; }
    }

    public class CommonTask
    {
        public int Completed { get; set; }
        public int Assigned { get; set; }
        public int ReadyForUAT { get; set; }
        public int Unassigned { get; set; }
        public int InProgress { get; set; }
    }

    public class CommonProject
    {
        public int Completed { get; set; }
        public int OnGoing { get; set; }
        public int NotStarted { get; set; }
    }
}
