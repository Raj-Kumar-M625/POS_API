using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using static IdentityServer4.Models.IdentityResources;

namespace ProjectOversight.API.Data.Repository
{
    public class UserStoryUIRepository : Repository<UserStoryUI>, IUserStoryUIRepository
    {
        private readonly ProjectOversightContext _posContext;
        public UserStoryUIRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
            _posContext = projectOversightContext;
        }

        public async Task<List<int?>> GetUIlist(int UserStoryId)
        {
            try
            {
                var codeValues = await _posContext.UserStoryUI.Where(x => x.UserStoryId == UserStoryId).Select(x => x.UIId).ToListAsync();
                return codeValues;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
