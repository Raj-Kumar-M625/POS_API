using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class CategoryRepository :Repository<Category> , ICategoryRepository
    {
        public CategoryRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
    }
}
