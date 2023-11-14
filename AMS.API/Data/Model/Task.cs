using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
#nullable disable
    public class Task: BaseEntity
    {
        
        [ForeignKey("UserStoryUI")]
        public int? UserStoryUIId { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectObjective")]
        public int? ProjectObjectiveId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public int? UserStoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal EstTime { get; set; }
        public decimal ActTime { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int Percentage { get; set; }
        public DateTime EstimateStartDate { get; set; }
        public DateTime EstimateEndDate { get; set; }
        public string TaskType { get; set; }
        public string Classification { get; set; }
        public int? TeamId { get; set; }
        public virtual UserStoryUI UserStoryUI { get; set; }
        public virtual Project Project { get; set; }
        public virtual Category Category { get; set; }
        //public virtual EmployeeTask EmployeeTask { get; set; }
        public string? TaskDescription { get; set; }



    }

}
