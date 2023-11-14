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
    public class PMOController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IPMOService _pmoService;

        public PMOController(IUnitOfWork repository, UserManager<User> userManager,
       IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, IPMOService pmoservicee)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _pmoService = pmoservicee;
        }


        [HttpPost("AddEmployeeScrumData")]
        public async Task<ActionResult<IEnumerable<bool>>> AddEmployeeScrumData()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _pmoService.AddEmployeeScrumData();
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


        [HttpGet("GetPMOList")]
        public async Task<ActionResult<PMOScrum>> GetPMOList(DateTime selectedDate)
        {
            try
            {
                var result = await _pmoService.GetPMOList(selectedDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetEmployeeYesterdayTaskDetails")]
        public async Task<EmployeeYesterdayTaskDetailsDTO> GetEmployeeYesterdayTaskDetails(DateTime selectedDate, int employeeId)
        {
            try
            {
                var result = await _pmoService.GetEmployeeYesterdayTaskDetails(selectedDate, employeeId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UpdateScrum")]
        public async Task<ActionResult<bool>> UpdateScrum(PMOScrumDto pMOScrumDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _pmoService.UpdateScrum(user, pMOScrumDto);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
