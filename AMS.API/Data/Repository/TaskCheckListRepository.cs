using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class TaskCheckListRepository : Repository<TaskCheckList>, ITaskCheckListRepository
    {
        public TaskCheckListRepository(ProjectOversightContext posContext) : base(posContext)
        {
        }
    }
}
