using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
  

    public class ProjectObjectiveRepository : Repository<ProjectObjective>, IProjectObjectiveRepository
    {
        public ProjectObjectiveRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
