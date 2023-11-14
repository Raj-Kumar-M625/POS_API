using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services;
using ProjectOversight.API.Services.Interface;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeDailyTaskController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmployeeDailyTaskService _dailyTaskService;
        public EmployeeDailyTaskController(IUnitOfWork repository, UserManager<User> userManager,
       IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager,IEmployeeDailyTaskService dailyTaskService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _dailyTaskService = dailyTaskService;
        }

        [HttpGet("GetTimePlanList")]
        public async Task<ActionResult<IEnumerable<EmployeeDailyTaskDto>>> GetTimePlanList()
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _dailyTaskService.GetTimePlanList(sessionUser);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeDailyTask")]
        public async Task<ActionResult<IEnumerable<EmployeeDailyTaskDto>>> GetEmployeeDailyTask(int EmployeeId)
        {
            try
            {

                var result = await _dailyTaskService.GetEmployeeTask(EmployeeId);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("AddEmployeeDailyTask")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeDailyTask>> AddEmployeeDailyTask(EmployeeDailyTaskDto employeeDailyTask)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _dailyTaskService.AddEmployeeDailyTask(user, employeeDailyTask);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("AddEmployeeDayPlan")]
        public async Task<ActionResult<EmployeeDailyTask>> AddEmployeeDayPlan(EmployeeDailyTaskDto employeeDailyTask)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _dailyTaskService.AddEmployeeDayPlan(user, employeeDailyTask);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeTimePlanList")]
        public async Task<ActionResult<List<EmployeeDailyTaskDto>>> GetEmployeeTimePlanList(int employeeId)
        {
            try
            {
                var result = await _dailyTaskService.GetEmployeeTimePlanList(employeeId);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetComments")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CommentsDto>>> GetComments()
        {
            try
            {
                var result = await _dailyTaskService.GetComments();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetEmployeeDailyTaskById")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeDailyTask>> GetEmployeeDailyTaskById(int employeeId, int projectId)
        {
            try
            {
                var result = await _dailyTaskService.GetEmployeeDailyTaskById(employeeId, projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeDailyTaskList")]
        [AllowAnonymous]
        public async Task<ActionResult<List<EmployeeDailyTask>>> GetEmployeeDailyTaskList(int employeeTaskId)
        {
            try
            {
                var result = await _dailyTaskService.GetEmployeeDailyTask(employeeTaskId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetCompletedWhatsapptaskListByTaskId")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeTask>> GetCompletedWhatsapptaskListByTaskId(int employeeId, DateTime WeekEndingDate)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _dailyTaskService.GetCompletedWhatsapptaskListByTaskId(employeeId, WeekEndingDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeTaskStat")]
        public async Task<ActionResult<IEnumerable<EmployeeDailyStatDto>>> GetEmployeeTaskStat(int employeeId, int employeeTaskId)
        {
            try
            {

                var result = await _dailyTaskService.GetEmployeeTaskStat(employeeId, employeeTaskId);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeDevChecklist")]
        public async Task<ActionResult<IEnumerable<Task>>> GetEmployeeDevChecklist(int TaskId)
        {
            try
            {

                var result = await _dailyTaskService.GetEmployeeDevChecklist(TaskId);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeTaskChecklist")]
        public async Task<ActionResult<IEnumerable<TaskCheckList>>> GetEmployeeTaskChecklist(int UserStoryUIID)
        {
            try
            {

                var result = await _dailyTaskService.GetEmployeeTaskChecklist(UserStoryUIID);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("AutoTaskChangeStatus")]
        public async Task<ActionResult<EmployeeDailyTask>> AutoTaskChangeStatus()
        {
            try
            {
                var result = await _dailyTaskService.AutoTaskChangeStatus();
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEachEmployeeDailyTaskById")]
        [AllowAnonymous]
        public async Task<ActionResult<EachEmployeeTask>> GetEachEmployeeDailyTaskById(int employeeId)
        {
            try
            {
                var result = await _dailyTaskService.GetEachEmployeeDailyTaskById(employeeId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw ;
            }

        }
        [HttpGet("GetMonthlyWiseEmployeeTasks")]
        [AllowAnonymous]

        public async Task<ActionResult<EachEmployeeTask>> GetMonthlyWiseEmployeeTasks(int employeeId)
        {
            try
            {
                var result = await _dailyTaskService.GetEmployeeMonthlyWise(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
