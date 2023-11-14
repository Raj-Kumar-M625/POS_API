using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Data.Repository
{
    public class TeamLeadRepository : Repository<TeamLead>, ITeamLeadRepository
    {
        public TeamLeadRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
