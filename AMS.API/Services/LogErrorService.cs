using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Services
{
    public class LogErrorService : ILogErrorService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        public LogErrorService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<bool> AddErrorLogs(LogErrorDto logErrorDto, User user)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                LogError logs = new()
                {
                    ControllerName = logErrorDto.ControllerName,
                    MethodName = logErrorDto.MethodName,
                    LogDescription = logErrorDto.LogDescription,
                    TableName = logErrorDto.TableName,
                    CreatedOn = DateTime.Now,
                    CreatedBy = employee.Name,
                };
                var addLogs = await _repository.LogError.CreateAsync(logs);

                return true;
            }
            catch
            {
                return true;
            }

        }
    }
}
