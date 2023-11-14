using Comments.API.Data.Models;
using Comments.API.Data.Repositories.Interface;
using Comments.API.Data.Repository.Interface;
using ProjectOversight.API.Data.Repository;
using Comment = Comments.API.Data.Models.Comments;

namespace Comments.API.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(CommentsContext commContext) : base(commContext)
        {
        }
    }
}
