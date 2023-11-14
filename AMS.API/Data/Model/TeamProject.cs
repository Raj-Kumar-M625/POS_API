using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class TeamProject : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public virtual Team? Team {get; set;}
        public virtual Project? Project { get; set; }
        [NotMapped]
        public string? ProjectName { get; set; }

    }
}
