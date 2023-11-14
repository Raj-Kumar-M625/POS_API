using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class EmployeeProjectRepository : Repository<EmployeeProject>, IEmployeeProjectRepository
    {
        private readonly ProjectOversightContext _posContext;
        public EmployeeProjectRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
            _posContext = projectOversightContext;
        }

        public async Task<List<int>> GetEmployeeProjectList(int empId)
        {
            var codeValues = await _posContext.EmployeeProject.Where(x => x.EmployeeId == empId ).Select(x => x.ProjectId).ToListAsync();
            return codeValues;
        }
    }
}
