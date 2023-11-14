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
    public class CommonController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly ICommonService _commonService;

        public CommonController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, ICommonService commonService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _commonService = commonService;
        }

        [HttpGet("GetCategoriesList")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesList()
        {
            try
            {
                var result = await _commonService.GetCategoriesList();
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
        [HttpGet("GetProjectTaskList")]
        public async Task<ActionResult<IEnumerable<Task>>> GetProjectTaskList(int ProjectId)
        {
            try
            {
                var result = await _commonService.GetProjectTaskList(ProjectId);
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
        [HttpGet("GetCommentsList")]
        public async Task<ActionResult<IEnumerable<CommentsDto>>> GetCommentsList(int EmployeeTaskId)
        {
            try
            {
                var result = await _commonService.GetCommentsList(EmployeeTaskId);
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

        [HttpPost("AddComment")]
        public async Task<ActionResult<bool>> AddComment(Comments comment)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _commonService.AddComment(user, comment);
                return Ok("Comment Created");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("AddReplyComments")]
        [AllowAnonymous]
        public async Task<ActionResult<Comments>> AddReplyComments(CommentsDto ComDetails)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _commonService.AddReplyComments(user, ComDetails);
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

        [HttpGet("GetDashboardData")]
        [AllowAnonymous]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _commonService.GetDashboardData(sessionUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("PerecentageCalculation")]

        public async Task<ActionResult> UpdatePercentage()
        {
            try
            {
                var result = await _commonService.UpdatePercentage();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("UpdateOutTime")]

        public async Task<ActionResult> UpdateOuttime()
        {
            try
            {
                var result = await _commonService.UpdateOutTime();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetTeamDashboardData")]
        [AllowAnonymous]
        public async Task<ActionResult<TeamDashboardDto>> GetTeamDashboardData(int teamId)
        {
            try
            {
                var sessionUser = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _commonService.GetTeamDashboardData(sessionUser,teamId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetAPPversion")]
        [AllowAnonymous]
        public async Task<ActionResult<AppVersion>> GetAppVersion(string VersionCode)
        {
            try
            {
                var result = await _repository.Appversion.GetAppVersionCode(VersionCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetTaskTypeClassification")]
        [AllowAnonymous]
        public async Task<ActionResult<TaskTypeClassification>> GetTaskTypeClassification()
        {
            try
            {
                var result = await _commonService.GetTasktypeClassificationList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
