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
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmployeeService _employeeService;
        private readonly ITaskService _taskService;

        public EmployeeController(IUnitOfWork repository, UserManager<User> userManager,
                                 IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager,
                                IEmployeeService employeeService, ITaskService taskService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _employeeService = employeeService;
            _taskService = taskService;
        }


        [HttpGet("GetEmployeeList")]
        public async Task<ActionResult<IEnumerable<EmployeeDailyTask>>> GetEmployeeList()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeService.GetEmployeeList();
                if (user.UserType == "Customer")
                {
                    var customerProjects = await _taskService.GetCustomerProject(user.Id);
                    var customerProjectIds = customerProjects.Select(cp => cp.ProjectId).ToList();
                    List<int> employeeId = new List<int>();
                    foreach (var customerProject in customerProjectIds)
                    {
                        var employeeproject = await _employeeService.GetEmployeeProject(customerProject);
                        employeeId.AddRange(employeeproject.Select(x => x.EmployeeId));
                    }
                    var filterEmployeeCustomerList = result.Where(x => employeeId.Contains(x.Id)).ToList();
                    return Ok(filterEmployeeCustomerList);
                }
                else
                {
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpPost("UpdateEmployee")]

        public async Task<ActionResult> UpdateEmployee(UserCreateDto userCreateDto)
        {
            try
            {
                var result = await _employeeService.UpdateEmployee(userCreateDto);
                return Ok("Employee update successfully!");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeStatDetails")]
        public async Task<ActionResult<EmployeeStatDto>> GetEmployeeStatDetails()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeService.GetEmployeeStatDetails(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AssignSkill")]
        public async Task<ActionResult> AssignSkill(EmployeeSkillSet[] empSkillSet)
        {
            try
            {
                var result = await _employeeService.AssignSkill(empSkillSet);
                return Ok("Employee Skill Updated!");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeSkillById")]
        public async Task<ActionResult<IEnumerable<EmployeeSkillSet>>> GetEmployeeSkillById(int employeeId)
        {
            try
            {
                var result = await _employeeService.GetEmployeeSkillById(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeTask")]
        public async Task<ActionResult<List<TeamDto>>> GetEmployeeTask( int teamId, DateTime? weekend, int ProjectId,DateTime? date)
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeService.GetEmployeeTask(sessionUser,teamId, weekend,ProjectId,date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeProject")]
        public async Task<ActionResult<List<EmployeeProject>>> GetEmployeeProject(int projectId)
        {
            try
            {
                var result = await _employeeService.GetEmployeeProject(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeAttendance")]
        public async Task<ActionResult<AttendanceDto>> GetEmployeeAttendance(DateTime selectedDate)
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeService.GetEmployeeAttendance(sessionUser, selectedDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetAttendanceByUserId")]
        public async Task<ActionResult<EmployeeAttendanceVM>> GetAttendanceByUserId(int userId, int month, int year)
        {
            try
            {
                var result = await _employeeService.GetAttendanceByUserId(userId, month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetEmployeeTasks")]
        public async Task<ActionResult<EmployeeTaskVM>> GetEmployeeTasks(int employeeId)
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeService.GetEmployeeTasks(sessionUser,employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetEmployeeTasksId")]
        public async Task<ActionResult<TaskDTO>> GetEmployeeTasksId(AttendenceFilterDto TaskList)
        {
            try
            {
                var result = await _employeeService.GetEmployeeTasksId(TaskList);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentImageDto uploadDto)
        {
            try
            {

                if (uploadDto.File == null) return new UnsupportedMediaTypeResult();
                {
                    var response = await _employeeService.UploadDocument(uploadDto);
                    return Ok(new { response.Id });
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost("DeleteImage")]
        public async Task<IActionResult> DeleteDocument(DocumentImageDto uploadDto)
        {
            try
            {

                if (uploadDto.File == null) return new UnsupportedMediaTypeResult();
                {
                    var response = await _employeeService.UploadDocument(uploadDto);
                    return Ok(new { response.Id });
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpGet("GetEmployeeTaskDetails")]
        public async Task<ActionResult<Task>> GetEmployeeTaskDetails(int employeeId, string status)
        {
            try
            {
                var result = await _employeeService.GetEmployeeTaskDetails(employeeId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetEmployeeTaskList")]
        public async Task<ActionResult<IEnumerable<Task>>> GetEmployeeTaskList(int Id, DateTime? weekend)
        {
            try
            {
                var result = await _employeeService.GetEmployeeTaskList(Id, weekend);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetEmployeeAttendence")]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetEmployeeAttendence(int Id, int? year, int? month)
        {
            try
            {
                var result = await _employeeService.GetEmployeeAttendence(Id, year, month);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeLoginDetails")]
        public async Task<ActionResult<IEnumerable<AttendenceVm>>> GetEmployeeLoginDetails(int Id, DateTime? weekend)
        {
            try
            {
                var result = await _employeeService.GetEmployeeLoginDetails(Id, weekend);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetEmployeeMonthlyTask")]
        public async Task<ActionResult<List<EachTeamMonthlyTask>>> GetEmployeeMonthlyTask(int teamId)
        {
            try
            {
                var result = await _employeeService.GetEmployeeMonthlyTask(teamId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeTeamList")]
        public async Task<ActionResult<IEnumerable<Team>>> GetEmployeeTeamList()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeService.GetEmployeeTeamList(user);
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

        [HttpGet("GetCustomerList")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetCustomerList()
        {
            try
            {
                var result = await _employeeService.GetCustomerList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetCustomerProject")]
        public async Task<ActionResult<List<CustomerProject>>> GetCustomerProject(int projectId)
        {
            try
            {
                var result = await _employeeService.GetCustomerProject(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
