using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Dto
{
    public class UserStoryUIDto
    {
        public int? UIId { get; set; }
        public int? UserStoryId { get; set; }
        public string UIName { get; set;}
        public string Status { get; set;}
    }
    public class UserStoryIdDto 
    {
        public int? userStoryId { get; set; }
        public string userStoryName { get; set; }
        public int? UIid { get; set; }
        public string name { get; set;}
        public string description { get; set;}
        public int percentage { get; set;}
        public string complexity { get; set;}
        public string uiCategory { get; set;}
        public string status { get; set;}
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
