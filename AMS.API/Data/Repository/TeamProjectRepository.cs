using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
   
    public class TeamProjectRepository : Repository<TeamProject>, ITeamProjectRepository
    {
        public TeamProjectRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
