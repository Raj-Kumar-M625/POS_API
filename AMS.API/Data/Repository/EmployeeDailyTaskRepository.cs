using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class EmployeeDailyTaskRepository : Repository<EmployeeDailyTask>, IEmployeeDailyTaskRepository
    {
        public EmployeeDailyTaskRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
