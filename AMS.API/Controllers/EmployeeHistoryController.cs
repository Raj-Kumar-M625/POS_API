using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeHistoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmployeeHistoryService _employeeHistoryService;
        private readonly ILogErrorService _logError;

        public EmployeeHistoryController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, IEmployeeHistoryService employeeHistoryService, ILogErrorService logError)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _employeeHistoryService = employeeHistoryService;
            _logError = logError;
        }
        [HttpGet("GetEmployeeHistory")]
        public async Task<ActionResult<IEnumerable<EmployeeDailyTask>>> GetEmployeeHistory(DateTime fromDate, DateTime toDate)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            try
            {
                var result = await _employeeHistoryService.GetEmployeeHistory(fromDate, toDate, user);
                if (result.Count > 0)
                    return Ok(result);
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                LogErrorDto logErrorDto = new LogErrorDto();
                logErrorDto.ControllerName = "Employee";
                logErrorDto.MethodName = "GetEmployeeHistory";
                logErrorDto.TableName = "TimePlan";
                logErrorDto.LogDescription = ex.Message + " , " + ex.InnerException != null ? ex.Message : ex.InnerException.Message;

                var result = await _logError.AddErrorLogs(logErrorDto, user);
                return Ok(result);
            }
        }
    }
}
