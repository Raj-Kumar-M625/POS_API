using Comments.API.Data.Repositories.Interface;
using Comments.API.Data.Repository.Interface;

namespace Comments.API.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CommentsContext _commentContext;
    private IUserRepository? _user;
    private IEmployeeRepository? _employee;
    private ICommentRepository? _comment;
    private ICommentHierarchyRepository? _commentHierarchy;
    private readonly IDisposable _dbdispose;
    private readonly DbFactory _dbFactory;

    public UnitOfWork(CommentsContext commentContext)
    {
        _commentContext = commentContext;
    }
    public IUserRepository User
    {
        get { return _user ??= new UserRepository(_commentContext); }
    }
    public IEmployeeRepository Employee
    {
        get { return _employee ??= new EmployeeRepository(_commentContext); }
    }
    public ICommentRepository Comment
    {
        get { return _comment ??= new CommentRepository(_commentContext); }
    }

    public ICommentHierarchyRepository CommentHierarchy
    {
        get { return _commentHierarchy ??= new CommentHierarchyRepository(_commentContext); }
    }

    public async Task<int> CommitAsync(CancellationToken token)
    {
        _dbFactory.DbContext.ChangeTracker.Entries();
        int ret = await _dbFactory.DbContext.SaveChangesAsync(token);
        _dbdispose.Dispose();
        return ret;
    }
}
