using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class UserInterfaceRepository : Repository<UserInterface>, IUserInterfaceRepository
    {
        public UserInterfaceRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
