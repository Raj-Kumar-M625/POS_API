using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class EmployeeSkillSet : BaseEntity
    {
        [ForeignKey("SkillSet")]
        public int SkillSetId { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
    }
}
