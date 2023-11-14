using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class UserTaskCheckListRepository : Repository<UserTaskCheckList>, IUserCheckListRepository
    {
        public UserTaskCheckListRepository(ProjectOversightContext posContext) : base(posContext) 
        {
        }
    }
}
