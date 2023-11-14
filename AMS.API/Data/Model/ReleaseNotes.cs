using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class ReleaseNotes:BaseEntity
    {
        public int? ProjectId { get; set; }
        public string? Version { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public bool? IsActive { get; set; }
        [NotMapped]
        public List<int>? TaskIdList { get; set; }

    }
}
