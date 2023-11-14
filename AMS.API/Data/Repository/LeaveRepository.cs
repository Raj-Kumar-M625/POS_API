using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class LeaveRepository : Repository<EmployeeLeave>, ILeaveRepository
    {
        public LeaveRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
