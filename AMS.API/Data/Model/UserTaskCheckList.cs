using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class UserTaskCheckList:BaseEntity
    {
        public int TaskId { get; set; }
        public int TaskCheckListId { get; set; }
        public int ProjectId { get; set; }
        public int USId { get; set; }
        public int UIId { get; set; }
        public int CategoryId { get; set; }
        public string CheckListDescription { get; set; }
        public bool IsDevChecked { get; set; }
        public bool IsQAChecked { get; set; }
        public bool IsLatest { get; set; }
        public int? UserStoryUIId { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public string? TaskName { get; set; }
    }
}
