using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class LeaveHistoryRepository : Repository<EmployeeLeaveHistory>, ILeaveHistoryRepository
    {
        public LeaveHistoryRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}

