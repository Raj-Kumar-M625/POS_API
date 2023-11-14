using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class UserStoryObjective : BaseEntity
    {
        [ForeignKey("Objective")]
        public int ObjectiveId { get; set; }
        [ForeignKey("UserStory")]
        public int UserStoryId { get; set; }
    }
}
