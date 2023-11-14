using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Data.Repository.Interface
{
    public interface IUserStoryUIRepository : IRepository<UserStoryUI>
    {
        Task<List<int?>> GetUIlist(int UserStoryId);
    }
}
