using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
   
    public class ProjectTechStackRepository : Repository<ProjectTechStack>, IProjectTechStackRepository
    {
        public ProjectTechStackRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
