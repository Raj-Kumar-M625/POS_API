using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        public ProjectController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, IProjectService projectService, ITaskService taskService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _projectService = projectService;
            _taskService = taskService;
        }
        [HttpGet("GetEmployeeProjectlist")]
        public async Task<ActionResult<IEnumerable<Project>>> GetEmployeeProjectlist()

        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.GetEmployeeProjectlist(user);
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
        [HttpPost("CreateEmployeeDayPlan")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeTask>> CreateEmployeeDayPlan(EmployeeTaskDto dayPlan)

        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.CreateEmployeeDayPlan(user, dayPlan);
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
        [HttpGet("GetAllProjectlist")]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjectlist()

        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.GetAllProjectlist();
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

        [HttpPost("GetAllProjectlistById")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjectlists(AttendenceFilterDto Tasklist)
        {
            try
            {
                var result = await _projectService.GetAllProjectlists(Tasklist);
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
        [HttpGet("GetUserInterfacelist")]
        public async Task<ActionResult<IEnumerable<UserStory>>> GetUserInterfacelist(int projectId)
        {
            try
            {

                var result = await _projectService.GetUserInterfacelist(projectId);
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

        [HttpPost("AddUserInterface")]
        public async Task<ActionResult<UserInterface>> AddUserInterface(UserInterface userInterface)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AddUserInterface(user, userInterface);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UpdateUserInterface")]
        public async Task<ActionResult<bool>> UpdateUserInterface(UserInterface userInterface)
        {
            try
            {
                var result = await _projectService.UpdateUserInterface(userInterface);
                if (result)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetProjectUSlist")]
        public async Task<ActionResult<IEnumerable<UserStory>>> GetProjectUSlist(int ProjectId)

        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.GetProjectUSlist(ProjectId);
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

        [HttpGet("GetCategoriesList")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesList()

        {
            try
            {
                //var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.GetCategoriesList();
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

        [HttpPost("AddEmployeeProject")]
        public async Task<ActionResult<IEnumerable<bool>>> AddEmployeeProject([FromBody] EmployeeProjectDto employeeProject)

        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AddEmployeeProject(user, employeeProject);
                if (result == true)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }


        }


        [HttpGet("GetProjectList")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectList()
        {
            try
            {
                var sessionuser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var projectList = await _projectService.GetProjectlist(sessionuser);
                if (sessionuser.UserType == "Customer")
                {
                    var customerProjects = await _taskService.GetCustomerProject(sessionuser.Id);
                    var customerProjectIds = customerProjects.Select(cp => cp.ProjectId).ToList();

                    var filteredProjects = projectList
                        .Where(project => customerProjectIds.Contains(project.Id))
                        .ToList();

                    if (filteredProjects.Any())
                    {
                        return Ok(filteredProjects);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else
                {
                    var result = await _projectService.GetProjectlist(sessionuser);
                    if (result.Count() > 0)
                        return Ok(result);
                    else
                        return NoContent();
                }
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetProjectById")]
        public async Task<ActionResult<Project>> GetProjectById(int Id)
        {
            try
            {
                var result = await _projectService.GetProjectById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpPost("AddProject")]
        public async Task<IActionResult> AddProject(Project projectData)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AddProject(user, projectData);
                if (result == true)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);
            }


        }
        [HttpPost("UpdateProject")]
        public async Task<IActionResult> UpdateProject(Project updatedProjectData)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.UpdateProject(user, updatedProjectData, updatedProjectData.Id);

                if (result)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("GetProjectObjective")]
        public async Task<ActionResult<ProjectObjectiveDto>> GetProjectObjective(int ProjectId)
        {
            try
            {
                var result = await _projectService.GetProjectObjective(ProjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving project objectives.");
            }
        }

        [HttpGet("GetProjectObjectiveById")]
        public async Task<ActionResult<IEnumerable<ProjectObjective>>> GetProjectObjectiveById(int Id)

        {
            try
            {
                var result = await _projectService.GetProjectObjectiveById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AddProjectObjective")]
        public async Task<IActionResult> AddProjectObjective(ProjectObjective projectObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AddProjectObjective(user, projectObjective);
                if (result == true)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("UpdateProjectObjective")]
        public async Task<IActionResult> UpdateProjectObjective(ProjectObjective updatedProjectObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.UpdateProjectObjective(user, updatedProjectObjective, updatedProjectObjective.Id);
                //var result = await _projectService.UpdateProjectObjective.FirstOrDefaultAsync(x => x.Id == updatedProjectObjective.Id);
                if (result)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetUserStoryList")]
        public async Task<ActionResult<IEnumerable<UserStory>>> GetUserStoryList(int projectId)
        {
            try
            {

                var result = await _projectService.GetUserStoryList(projectId);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("AddUserStory")]
        public async Task<ActionResult> AddUserStory(UserStory UserStory)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AddUserStory(user, UserStory);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UploadFiles")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadFiles([FromForm] Document document)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.UploadFiles(user, document);
                if (result == true) 
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("DownloadFile")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            try
            {
                var result = await _projectService.DownloadFile(id);
                Response.Headers.Add("Content-Disposition", $"attachment; filename={result.FileName}");
                Response.Headers.Add("Content-Type", "application/octet-stream");
                return File(result.FileContent, "application/octet-stream", result.FileName);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteFile")]
        public async Task<ActionResult> DeleteFile(int id)
        {
            try
            {
               var result = await _projectService.DeleteFile(id);
               return Ok(result);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UpdateUserStory")]
        public async Task<ActionResult> UpdateUserStory(UserStory updatedUserStory)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.UpdateUserStory(user, updatedUserStory);
                if (result == true)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AddUserStoryUI")]
        public async Task<ActionResult<bool>> AddUserStoryUI(UserStoryUI[] userStoryUI)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AddUserStoryUI(user, userStoryUI);
                if (result == true)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetUserStoryUIList")]
        public async Task<ActionResult<List<UserStoryUI>>> GetUserStoryUIList(int userStoryId)
        {
            try
            {
                var result = await _projectService.GetUserStoryUIList(userStoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AssignEmployeeProject")]
        public async Task<ActionResult<bool>> AssignEmployeeProject(EmployeeProject[] employeeProject)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AssignEmployeeProject(user, employeeProject);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AssignLead")]
        public async Task<ActionResult<bool>> AssignLead(int userId, int projectId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AssignLead(user, userId, projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetUnAssignedProjects")]
        public async Task<ActionResult<List<Project>>> GetUnAssignedProjects(int TeamID)
        {
            try
            {
                var result = await _projectService.GetUnAssignedProjects(TeamID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetProjectStatDetails")]
        public async Task<ActionResult<ProjectStatDto>> GetProjectStatDetails(int ProjectId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.GetProjectStatDetails(ProjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetAllUserStory")]
        public async Task<ActionResult<List<UserStory>>> GetAllUserStory(int projectId)
        {
            try
            {
                var result = await _projectService.GetAllUserStory(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getProjectDashBoardData")]
        public async Task<ActionResult<ProjectDashboardDto>> getProjectDashBoardData()
        {
            try
            {
                var result = await _projectService.getProjectDashBoardData();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getCheckListProject")]
        public async Task<ActionResult<ProjectCheckList>> getCheckListProject()
        {
            try
            {
                var result = await _projectService.getCheckListProject();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEachProjectCategories")]
       
        public async Task<ActionResult<ProjectCatergory>> getEachProjectCategories(int projectId)
        {
            try
            {
                var result = await _projectService.getEachProjectCategories(projectId);
                return Ok(result);
            }
            catch (Exception ex)    
            {
                throw ex;
            }
        }
        [HttpPost("AssignCustomerProject")]
        public async Task<ActionResult<bool>> AssignCustomerProject(CustomerProject[] customerProject)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _projectService.AssignCustomerProject(user, customerProject);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("getCustomerProjectReport")]
        public async Task<ActionResult<ReportWithCounts>> getCustomerProjectReport (int projectId)
        {
            try
            {
                var result = await _projectService.getCustomerProjectReport(projectId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetProjectReportDetails")]
        public async Task<ActionResult<ProjectReportVM>> GetProjectReportDetails(int projectId)
        {
            try
            {
                var result = await _projectService.GetProjectReportDetails(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
