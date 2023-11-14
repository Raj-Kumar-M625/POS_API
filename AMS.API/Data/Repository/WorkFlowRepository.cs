using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class WorkFlowRepository : Repository<WorkFlow>, IWorkFlowRepository
    {
        public WorkFlowRepository(ProjectOversightContext posContext) : base(posContext)
        {
        }
    }
}
