using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Data.Repository.Interface
{
    public interface IEmployeeProjectRepository : IRepository<EmployeeProject>
    {
        Task<List<int>> GetEmployeeProjectList(int empId);
    }
}
