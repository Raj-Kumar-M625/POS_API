using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class LogErrorRepository : Repository<LogError>, ILogErrorRepository
    {
        public LogErrorRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
