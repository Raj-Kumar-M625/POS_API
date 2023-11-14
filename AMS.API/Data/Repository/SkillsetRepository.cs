using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
//using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Data.Repository
{
    public class SkillsetRepository : Repository<ProjectOversight.API.Data.Model.SkillSet>, ISkillsetRepository
    {
        public SkillsetRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }

     
    }
}
