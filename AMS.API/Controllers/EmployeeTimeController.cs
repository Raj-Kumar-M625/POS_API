using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data;
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
    public class EmployeeTimeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmployeeTimeService _employeeTimeService;

        public EmployeeTimeController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, IEmployeeTimeService employeeTimeService
          
            )
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _employeeTimeService = employeeTimeService;
        }

        [HttpPost("AddEmployeeTimeDetails")]
        public async Task<ActionResult<IEnumerable<bool>>> AddEmployeeTimeDetails([FromBody] EmployeeTimeDto LoginDetails)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeTimeService.AddEmployeeTimeDetails(user, LoginDetails);
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

        [HttpGet("GetEmployeeTimeDetails")]
        public async Task<ActionResult<IEnumerable<EmployeeTime>>> GetEmployeeTimeDetails()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeTimeService.GetEmployeeTimeDetails(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetComments")]
        public async Task<ActionResult<List<CommentsDto>>> GetComments()
        {
            try
            {
                var result = await _employeeTimeService.GetComments();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetWorkflow")]
        public async Task<ActionResult<List<WorkflowDto>>>GetWorkFlow(int employeeId, string date)
        {
            try
            {
                var result = await _employeeTimeService.GetWorkflows(employeeId,date);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
