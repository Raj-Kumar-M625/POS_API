using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    

    public class TeamRepository : Repository<ProjectOversight.API.Data.Model.Team>, ITeamRepository
    {
        public TeamRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
