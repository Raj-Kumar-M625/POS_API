using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class EmployeeWorkFlowRepository : Repository<EmployeeWorkFlow>, IEmployeeWorkFlowRepository
    {
        public EmployeeWorkFlowRepository(ProjectOversightContext posContext) : base(posContext)
        {
        }
    }
}
