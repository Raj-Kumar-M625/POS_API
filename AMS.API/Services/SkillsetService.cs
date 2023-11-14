using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using Task = ProjectOversight.API.Data.Model.Task;


namespace ProjectOversight.API.Services
{
    public class Skillsetservice : ISkillsetService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public int Id { get; private set; }

        public Skillsetservice(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }



        public async Task<List<SkillSet>> GetSkillsetList()
        {
            try
            {
                var SkillsetList = await _repository.skillset.FindAllAsync();
                var result = SkillsetList.OrderByDescending(x => x.Id).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
                

        public async Task<SkillSet> GetSkillsetById(int Id)
        {
            try
            {
                var Skillset = await _repository.skillset.FindByConditionAsync(x => x.Id == Id);
                var skill = Skillset.FirstOrDefault();
                return skill;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AddSkillset(User user, SkillsetDto SkillSet)
        {
            try
            {               
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                SkillSet myskillSet = new()
                {
                    Category = SkillSet.Category,
                    CreatedDate = DateTime.Now,
                    CreatedBy = employee.UserId.ToString(),                   
                    UpdatedBy = employee.UserId.ToString(),
                    UpdatedDate = DateTime.Now,
                    SubCategory1 = SkillSet.SubCategory1,
                    SubCategory2 = SkillSet.SubCategory2 == "string" ? string.Empty : SkillSet.SubCategory2,
                    SubCategory3 = SkillSet.SubCategory3 == "string" ? string.Empty : SkillSet.SubCategory3,


                };
                var Skillset = await _repository.skillset.CreateAsync(myskillSet);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        public async Task<bool> UpdateSkillset(User user, SkillsetDto SkillSet)
        {
            var skillsetValue = await _repository.skillset.FindById(x => x.Id == SkillSet.Id);
            var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
            if (skillsetValue != null)
            {
                skillsetValue.Category = SkillSet.Category;
                skillsetValue.SubCategory1 = SkillSet.SubCategory1;
                skillsetValue.SubCategory2 = SkillSet.SubCategory2 == "string" ? string.Empty : SkillSet.SubCategory2;
                skillsetValue.SubCategory3 = SkillSet.SubCategory3 == "string" ? string.Empty : SkillSet.SubCategory3;
                skillsetValue.UpdatedBy = employee.UserId.ToString();
                skillsetValue.UpdatedDate = DateTime.Now;
                await _repository.skillset.UpdateAsync(skillsetValue);
                return true;
            }
            else
            {
                return false;
            }

        }
    



    }   
}
