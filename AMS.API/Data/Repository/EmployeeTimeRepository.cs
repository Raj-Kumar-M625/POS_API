using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class EmployeeTimeRepository : Repository<EmployeeTime>, IEmployeTimeRepository
    {
        public EmployeeTimeRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
