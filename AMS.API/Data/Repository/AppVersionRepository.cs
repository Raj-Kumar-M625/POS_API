using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;

namespace ProjectOversight.API.Data.Repository
{
    public class AppVersionRepository : Repository<AppVersion>, IAppversionRepository
    {
        public AppVersionRepository(ProjectOversightContext projectOversightContext) : base(projectOversightContext)
        {
        }
        public async Task<bool> GetAppVersion(string versionCode)
        {
            var codeValues = await FindByConditionAsync(x => x.VersionCode == versionCode && DateTime.Now >= x.CreatedDate && DateTime.Now <= x.ExpiryDate);
            if (codeValues.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<AppVersion> GetAppVersionCode(string versionCode)
        {
            var codeValues = await FindByConditionAsync(x => x.VersionCode == versionCode && DateTime.Now >= x.CreatedDate && DateTime.Now <= x.ExpiryDate);
            return codeValues.LastOrDefault();
        }
    }
}
