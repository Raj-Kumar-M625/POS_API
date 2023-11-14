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
    public class TeamAttendenceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly ITeamAttendenceService _teamAttendenceService;

        public TeamAttendenceController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, ITeamAttendenceService teamAttendenceService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _teamAttendenceService = teamAttendenceService;
        }

        [HttpGet("GetTeamDetailsById")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeamDetailsById(int employeeId)
        {
            try
            {
                var result = await _teamAttendenceService.GetTeamDetailsById(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetTeamLoginDetails")]
        public async Task<ActionResult<TeamAttendenceDto>> GetTeamLoginDetails(int teamId)
        {
            try
            {
                var result = await _teamAttendenceService.GetTeamLoginDetails(teamId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetTeamAttendenceStatistics")]
        public async Task<ActionResult<IEnumerable<TeamAttendenceDto>>> GetTeamAttendenceStatistics(int teamId)
        {
            try
            {
                var result = await _teamAttendenceService.GetTeamAttendenceStatistics(teamId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost("GetMemberAttendenceList")]
        public async Task<ActionResult<IEnumerable<TeamAttendenceDto>>> GetMemberAttendenceList(AttendenceFilterDto attendenceFilter)
        {
            try
            {
                    var result = await _teamAttendenceService.GetMemberAttendenceList(attendenceFilter);   
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetMemberAttendenceStat")]
        public async Task<ActionResult<IEnumerable<TeamAttendenceDto>>> GetMemberAttendenceStat(int employeeId)
        {
            try
            {
                var result = await _teamAttendenceService.GetMemberAttendenceStat(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }






    }
}
