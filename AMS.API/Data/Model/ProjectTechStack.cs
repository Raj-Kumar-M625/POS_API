using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class ProjectTechStack :BaseEntity
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [ForeignKey("CommonMaster")]
        public int  TechStack { get; set; }  
        public virtual CommonMaster CommonMaster { get; set; }

    }
}
