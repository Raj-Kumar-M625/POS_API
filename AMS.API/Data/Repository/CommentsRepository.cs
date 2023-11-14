using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class CommentsRepository : Repository<Comments>, ICommentsRepository
    {
        public CommentsRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
