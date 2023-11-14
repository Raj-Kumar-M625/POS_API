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

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly ITeamService _teamService;

        public TeamController(IUnitOfWork repository, UserManager<User> userManager,
       IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, ITeamService teamService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _teamService = teamService;
        }

        [HttpGet("GetTeamList")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamList()
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.GetTeamList(sessionUser);
                
                    return Ok(result);
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AddTeam")]
        public async Task<ActionResult<IEnumerable<bool>>> AddTeam(Team team)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.AddTeam(user, team);
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


        [HttpGet("GetTeamMemberList")]
        public async Task<ActionResult<IEnumerable<TeamEmployee>>> GetTeamMemberList()
        {
            try
            {

                var result = await _teamService.GetTeamMemberList();
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

        [HttpGet("GetTeamNames")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeamNames(int projectId)
        {
            try
            {

                var result = await _teamService.GetTeamNames(projectId);
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

        [HttpGet("GetTeamById")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamById(int Id)
        {
            try
            {
                var result = await _teamService.GetTeamById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet("GetTeamEmployeelist")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeamEmployeelist(int teamId)
        {
            try
            {
                var result = await _teamService.GetTeamEmployeelist(teamId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AssignEmployeeToTeam")]
        public async Task<ActionResult<bool>> AssignEmployeeToTeam(TeamEmployee[] teamEmployee)
        {
            try
            {
                var result = await _teamService.AssignEmployeeToTeam(teamEmployee);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AddTeamObjective")]
        public async Task<IActionResult> AddTeamObjective([FromBody] TeamObjective teamObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.AddTeamObjective(user, teamObjective);
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

        [HttpPost("UpdateTeamObjective")]
        public async Task<IActionResult> UpdateTeamObjective([FromBody] TeamObjective updatedObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.UpdateTeamObjective(user, updatedObjective);
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


        [HttpGet("GetTeamObjectiveList")]
        public async Task<ActionResult<IEnumerable<TeamObjective>>> GetTeamObjectiveList(int teamId)

        {
            try
            {
                var result = await _teamService.GetTeamObjectiveList(teamId);
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


        [HttpGet("GetTeamObjectiveById")]
        public async Task<ActionResult<IEnumerable<TeamObjective>>> GetTeamObjectiveById(int Id)

        {
            try
            {
                var result = await _teamService.GetTeamObjectiveById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetTeamProjectList")]
        public async Task<ActionResult<List<TeamProjectDto>>> GetTeamProjectList(int teamId)
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.GetTeamProjectList(sessionUser,teamId);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetTeamEmployeeTaskList")]
        public async Task<ActionResult<IEnumerable<EmployeeTask>>> GetTeamEmployeeTaskList(int Id)
        {
            try
            {
                var result = await _teamService.GetTeamEmployeeTaskList(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("Updateteam")]
        public async Task<ActionResult<bool>> Updateteam(Team team)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.Updateteam(user, team);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("AddTeamProject")]
        public async Task<ActionResult<bool>> AddTeamProject(TeamProject[] teamProject)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.AddTeamProject(user, teamProject);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetProjectList")]
        public async Task<ActionResult<List<Project>>> GetProjectList(int teamId)
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.GetProjectList(teamId, sessionUser);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("AddTeamWeeklyObjective")]
        public async Task<ActionResult<bool>> AddTeamWeeklyObjective(TeamWeeklyObjective teamWeeklyObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.AddTeamWeeklyObjective(user, teamWeeklyObjective);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetTeamWeeklyObjective")]
        public async Task<ActionResult<List<TeamWeeklyObjective>>> GetTeamWeeklyObjective(int teamId)
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.GetTeamWeeklyObjective(sessionUser,teamId);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("UpdateTeamWeeklyObjective")]
        public async Task<IActionResult> UpdateTeamWeeklyObjective(TeamWeeklyObjective updatedObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.UpdateTeamWeeklyObjective(user, updatedObjective);
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

        [HttpPost("AddTeamMonthlyObjective")]
        public async Task<ActionResult<bool>> AddTeamMonthlyObjective(TeamMonthlyObjective teamMonthlyObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.AddTeamMonthlyObjective(user, teamMonthlyObjective);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetTeamMonthlyObjective")]
        public async Task<ActionResult<List<TeamMonthlyObjective>>> GetTeamMonthlyObjective(int teamId)
        {
            try
            {
                var result = await _teamService.GetTeamMonthlyObjective(teamId);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("UpdataTeamMonthlyObjective")]
        public async Task<ActionResult<bool>> UpdataTeamMonthlyObjective(TeamMonthlyObjective teamMonthlyObjective)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _teamService.UpdataTeamMonthlyObjective(user, teamMonthlyObjective);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetTeamleavedetails")]
        public async Task<ActionResult<TeamLeaveVM>> GetTeamleavedetails(int TeamId, int year, int month)
        {
            try
            {
                var result = await _teamService.GetTeamleavedetails(TeamId, year, month);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
    }
}
