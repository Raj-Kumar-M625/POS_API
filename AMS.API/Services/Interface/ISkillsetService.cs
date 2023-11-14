using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using SkillSet = ProjectOversight.API.Data.Model.SkillSet;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services.Interface
{
    public interface ISkillsetService
    {
        Task<List<SkillSet>> GetSkillsetList();
        Task<SkillSet> GetSkillsetById(int Id);

        Task<bool> AddSkillset(User user, SkillsetDto SkillsetDto);
        
      
        
        Task<bool> UpdateSkillset(User user, SkillsetDto skillset);
       
    }


}
