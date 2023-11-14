using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class UserStoryUI :BaseEntity
    {
     
        [ForeignKey("UserInterface")]
        public int? UIId { get; set; }
        [ForeignKey("UserStory")]
        public int? UserStoryId { get; set; }
       public virtual UserInterface? UserInterface { get; set; }
        public virtual UserStory? UserStory { get; set; }
    }

}
