using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class UserStoryRepository : Repository<UserStory>, IUserStoryRepository
    {
        public UserStoryRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
