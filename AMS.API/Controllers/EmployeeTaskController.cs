using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Services;
using ProjectOversight.API.Services.Interface;
using Task = ProjectOversight.API.Data.Model.Task;
using System.Threading.Tasks;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeTaskController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmployeeTaskService _employeeTaskService;
        public EmployeeTaskController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, IEmployeeTaskService employeeTaskService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _employeeTaskService = employeeTaskService;
        }

        [HttpGet("GetProjectTasklist")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetProjectTasklist(int Id)
        {
            try
            {
                var result = await _employeeTaskService.GetProjectTasklist(Id);
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

        [HttpGet("GetProjectTaskById")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Task>>> GetProjectTaskById(int Id)
        {
            try
            {
                var result = await _employeeTaskService.GetProjectTaskById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("AssignEmployeeTask")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeTask>> AssignEmployeeTask(EmployeeTaskDto dayPlan)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeTaskService.AssignEmployeeTask(user, dayPlan);
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
        [HttpGet("GetEmployeeTaskbyId")]
        [AllowAnonymous]
        public async Task<bool> GetEmployeeTaskbyId(int EmployeeId, int TaskId)
        {
            try
            {
                var result = await _employeeTaskService.GetEmployeeTaskbyId(EmployeeId, TaskId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetTaskDetalisById")]
        [AllowAnonymous]
        public async Task<ActionResult<Task>> GetTaskDetalisById(int TaskId)
        {
            try
            {
                var result = await _employeeTaskService.GetTaskDetalisById(TaskId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetComments")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentsDto>>> GetComments()
        {
            try
            {
                var result = await _employeeTaskService.GetComments();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetAssignedEmployeeTaskById")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeTask>> GetAssignedEmployeeTaskById(int TaskId, int projectId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeTaskService.GetAssignedEmployeeTaskById(TaskId, projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetWhatsapptaskListByTaskId")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeTask>> GetWhatsapptaskListByTaskId( int employeeId, DateTime WeekEndingDate)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeTaskService.GetWhatsapptaskListByTaskId( employeeId, WeekEndingDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("MoveTask")]
        public async Task<ActionResult<EmployeeTask>> MoveTask(EmployeeTaskDto employeeTaskDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeTaskService.MoveTask(user, employeeTaskDto);
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

        [HttpPost("ReassignTask")]
        public async Task<ActionResult<bool>> ReassignTask(EmployeeTaskDto employeeTaskDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeTaskService.ReassignTask(user, employeeTaskDto);
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
    }
}
