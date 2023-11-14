using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Data.Repository.Interface
{
    public interface  IAppversionRepository : IRepository<AppVersion>
    {
        Task<bool> GetAppVersion(string versionCode);
        Task<AppVersion> GetAppVersionCode(string versionCode);
    }
}

