using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class TeamEmployeeRepository : Repository<TeamEmployee>, ITeamEmployeeRepository 
    {
        public TeamEmployeeRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
