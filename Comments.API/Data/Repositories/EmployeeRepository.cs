using Comments.API.Data.Models;
using Comments.API.Data.Repository.Interface;
using ProjectOversight.API.Data.Repository;

namespace Comments.API.Data.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CommentsContext commentContext) : base(commentContext)
        {
        }
    }
}
