using Comments.API.Data.Models;
using Comment = Comments.API.Data.Models.Comments;

namespace Comments.API.Services.Interface
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentHierarchy>> GetComments();
        Task<bool> AddComment(User user,CommentHierarchy comment);
        Task<bool> UpdateComment(User user,CommentHierarchy commentHierarchy);
    }
}
