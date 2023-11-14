using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class EmployeeTaskRepository : Repository<EmployeeTask>, IEmployeeTaskRepository
    {
        public EmployeeTaskRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
