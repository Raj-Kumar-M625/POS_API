using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Dto
{
    public class EmployeeTaskDto
    {        
        public int EmployeeId { get; set; }  
        public int EmployeeTaskId { get; set; }
        public int Id { get; set; }
        public int TaskId { get; set; }       
        public int ProjectId { get; set; }      
        public int? ProjectObjectiveId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int Percentage { get; set; }
        public decimal? AssignedHours { get; set; }
        public decimal? UnassignedHours { get; set; }
        public string? Name { get; set; }
        public DateTime EstStartDate { get; set; }
        public DateTime EstEndDate { get; set; }
        public string? Description { get; set; }
        public string? Comment { get; set; }
        public string? CreatedBy { get; set; }
        public string? TaskDescription { get; set; }
        public string? ProjectName { get; set; }
        public string? UserStory { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string? UserInterface { get; set; }
        public DateTime? WorkedOn { get; set; }
    }

    public class EmployeeTaskVM
    {
       public List<Project> EmployeeProjects { get; set; }
       public List<EmployeeTaskDto> EmployeeDailyTask { get; set; }
       public int? TeamId { get; set; } 
    }
}
