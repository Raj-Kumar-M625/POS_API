using System.ComponentModel.DataAnnotations.Schema;

namespace Comments.API.Data.Models
{
    public class CommentHierarchy:BaseEntity
    {
        public string TableName { get; set; }
        public string? ProjectName { get; set; }
        public int AttributeId { get; set; }
        public int DisplaySequence { get; set; }
        public string LiteralName { get; set; }
        [NotMapped]
        public string Description { get; set; }
    }
}
 