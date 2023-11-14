namespace Comments.API.Data.Models
{
    public class Comments:BaseEntity
    {
        public int CommentHierarchyId { get; set; } 
        public string Description { get; set; 
        }
    }
}
