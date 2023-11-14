using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Services.Interface
{
    public interface ICommonMasterService
    {
        Task<List<CommonMaster>> GetCodeTableList();
        Task<bool> UpdateCommonMaster(User user, CommonMaster commonMaster);
        Task<bool> AddCommonmaster(User user, CommonMaster commonMaster);
    }
}
