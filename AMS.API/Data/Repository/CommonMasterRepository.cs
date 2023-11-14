using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class CommonMasterRepository: Repository<CommonMaster>, ICommonMasterRepository
    {
        public CommonMasterRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
