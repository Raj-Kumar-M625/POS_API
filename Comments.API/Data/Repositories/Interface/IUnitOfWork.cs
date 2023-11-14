using Comments.API.Data.Repositories.Interface;

namespace Comments.API.Data.Repository.Interface
{
    public interface IUnitOfWork 
    {
        IUserRepository User { get; }
        IEmployeeRepository Employee { get; }
        ICommentRepository Comment { get; }
        ICommentHierarchyRepository CommentHierarchy { get; }
        Task<int> CommitAsync(CancellationToken token);
      
    }
}
