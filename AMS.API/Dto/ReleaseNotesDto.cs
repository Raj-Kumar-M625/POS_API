using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Dto
{
    public class ReleaseNotesDto
    {
        public int? ProjectId { get; set; }
        public string? Version { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public bool? IsActive { get; set; }
        public List<Data.Model.Task> Tasks { get; set; }
    }
}
