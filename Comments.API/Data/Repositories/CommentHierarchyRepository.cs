using Comments.API.Data.Models;
using Comments.API.Data.Repositories.Interface;
using ProjectOversight.API.Data.Repository;

namespace Comments.API.Data.Repositories
{
    public class CommentHierarchyRepository : Repository<CommentHierarchy>, ICommentHierarchyRepository
    {
        public CommentHierarchyRepository(CommentsContext commContext) : base(commContext)
        {
        }
    }
}
