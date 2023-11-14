using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ReleaseNotesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IReleaseNotesService _releaseNotesService;
        public ReleaseNotesController(IUnitOfWork repository, UserManager<User> userManager,
                              IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager,
                             IReleaseNotesService releaseNotesService)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _releaseNotesService = releaseNotesService;
        }

        [HttpGet("GetReadyForUATTaskList")]
        public async Task<ActionResult<IEnumerable<Data.Model.Task>>> GetReadyForUATTaskList(int projectId)
        {
            try
            {
                var result = await _releaseNotesService.GetAllReadyForUATTasklist(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("UpdateInUATTaskList")]
        public async Task<ActionResult<IEnumerable<Data.Model.Task>>> UpdateInUATTaskList(List<int> projectId)
        {
            try
            {
                var result = await _releaseNotesService.UpdateInUATTask(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
              
        [HttpPost("AddReleaseNotes")]
        public async Task<ActionResult<bool>> AddReleaseNotes(ReleaseNotes releaseNotes)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _releaseNotesService.AddReleaseNotes(user,releaseNotes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetReleaseNotes")]
        public async Task<ActionResult<ReleaseNotes>> GetReleaseNotes(int projectId)
        {
            try
            {
                var result = await _releaseNotesService.GetReleaseNotes(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetReleaseNotesHistory")]
        public async Task<ActionResult<List<ReleaseNotesDto>>> GetReleaseNotesHistory(int projectId)
        {
            try
            {
                var result = await _releaseNotesService.GetReleaseNotesHistory(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
