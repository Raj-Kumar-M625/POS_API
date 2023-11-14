using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Dto
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        [Range(0, 100)]
        public int Percentage { get; set; }
        public virtual ProjectDocuments? ProjectDocuments { get; set; }
        public virtual ICollection<ProjectTechStack>? ProjectTechStacks { get; set; }
        [NotMapped]
        public int[]? TechStackId { get; set; }
        [NotMapped]
        public int UserStoryCount { get; set; }
        [NotMapped]
        public int UseInterfaceCount { get; set; }
        [NotMapped]
        public int? TotalTaskCount { get; set; }
        [NotMapped]
        public int InProgressCount { get; set; }
        [NotMapped]
        public int NotStartedTaskCounts { get; set; }
        [NotMapped]
        public int TeamId { get; set; }
        public string? ProjectName { get;set; }
        public decimal? AssignedHours { get; set; }
        public int? AssignedTaskCount { get; set; }
        public int? TotalCompletedCount { get; set; }
        public int? TotalInProgressCount { get; set; }
        public int? ProjectCount { get; set; }
        public List<string>? condition { get; set; }
        public List<int>? month { get; set; }
        public List<string>? weekenddate { get; set; }

    }

    public class ProjectReportVM
    {
        public List<TaskListDto> HighPriorityTasks { get; set; }
        public List<TaskListDto> TodaysTasks { get; set; }
        public List<UserStory> UserStory { get; set; }
        public List<UserInterface> UserInterface { get; set; }
        public List<Task> Task { get; set; }
        public int TotalResource { get; set; }
        public decimal TotalResourceHours { get; set; }
        public decimal CompletedPercentage { get; set; }
        public decimal PendingPercentage { get; set; }
    }
}
