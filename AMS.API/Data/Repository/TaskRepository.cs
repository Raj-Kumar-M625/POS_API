using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class TaskRepository : Repository<ProjectOversight.API.Data.Model.Task>, ITaskRepository
    {
        public TaskRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
