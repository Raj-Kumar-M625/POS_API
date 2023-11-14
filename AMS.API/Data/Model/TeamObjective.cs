using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class TeamObjective : BaseEntity
    {
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public string Description { get; set; }
        public string? Status { get; set; }
        public int? Percentage { get; set; }
    }
}
