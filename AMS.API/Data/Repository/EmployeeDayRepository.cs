using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class EmployeeDayRepository : Repository<EmployeeDay>, IEmployeeDayRepository
    {
        public EmployeeDayRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
