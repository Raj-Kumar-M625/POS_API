using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class TaskTypeClassificationRepository : Repository<TaskTypeClassification>, ITaskTypeClassificationRepository
    {
        public TaskTypeClassificationRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
