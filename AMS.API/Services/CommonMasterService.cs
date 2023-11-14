using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Services
{
    public class CommonMasterService : ICommonMasterService
    {

        private readonly ProjectOversightContext _dbContext;
        public CommonMasterService(ProjectOversightContext dbContext)
        {

            _dbContext = dbContext;
        }
        public async Task<List<CommonMaster>> GetCodeTableList()
        {
            try
            {
                var codeTable = await _dbContext.CommonMaster.OrderByDescending(x => x.Id).ToListAsync();
                return codeTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateCommonMaster(User user, CommonMaster commonMaster)
        {
            try
            {
                var updateCommonMaster = await _dbContext.CommonMaster.FirstOrDefaultAsync(x => x.Id == commonMaster.Id);
                if (updateCommonMaster != null)
                {
                    updateCommonMaster.CodeType = commonMaster.CodeType;
                    updateCommonMaster.CodeName = commonMaster.CodeName;
                    updateCommonMaster.CodeValue = commonMaster.CodeValue;
                    updateCommonMaster.DisplaySequence = commonMaster.DisplaySequence;
                    updateCommonMaster.IsActive = commonMaster.IsActive;
                    _dbContext.CommonMaster.Update(updateCommonMaster);
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        public async Task<bool> AddCommonmaster(User user, CommonMaster commonMaster)
        {
            try
            {
                CommonMaster common = new CommonMaster()
                {
                    CodeType = commonMaster.CodeType,
                    CodeName = commonMaster.CodeName,
                    CodeValue = commonMaster.CodeValue,
                    DisplaySequence = commonMaster.DisplaySequence,
                    IsActive = commonMaster.IsActive,
                };
                var CommonDetails = await _dbContext.CommonMaster.AddAsync(common);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
    }
}

