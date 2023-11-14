using Comments.API.Data.Repository.Interface;
using Comment = Comments.API.Data.Models.Comments;

namespace Comments.API.Data.Repositories.Interface
{
    public interface ICommentRepository : IRepository<Comment>
    {
    }
    
}
