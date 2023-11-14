using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeLeaveController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmployeeLeaveService _employeeLeave;
        public EmployeeLeaveController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager, IEmployeeLeaveService employeeLeave)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _employeeLeave = employeeLeave;
        }


        [HttpPost("ApplyEmployeeLeave")]
        public async Task<ActionResult> ApplyEmployeeLeave(EmployeeLeaveDto leaveDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeLeave.ApplyLeaveRequest(user, leaveDto);
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


        [HttpPost("EditLeaveRequest")]
        public async Task<ActionResult> EditLeaveRequest(EmployeeLeaveDto leaveDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeLeave.EditLeaveRequest(user, leaveDto);
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

        [HttpGet("GetAllEmployeeLeaveList")]
        public async Task<ActionResult<EmployeeLeave>> GetAllEmployeeLeaveList(int employeeId)
        {
            try
            {
                var result = await _employeeLeave.GetAllEmployeeLeaveList(employeeId);
                return Ok(result);

            }catch(Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetAllLeaveHistoryById")]

        public async Task<ActionResult<LeaveHistoryDto>> GetAllLeaveHistoryById(int employeeId, int selectedId)
        {
            try
            {
                var result = await _employeeLeave.GetAllEmployeeLeaveById(employeeId, selectedId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetLeaveByTeamId")]
        public async Task<ActionResult<EmployeeLeaveDto>> GetLeaveByTeamId(int teamid)
        {
            try
            {
                var result = await _employeeLeave.GetLeaveByTeamId(teamid);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetLeaveList")]
        public async Task<List<LeaveHistoryDto>> GetLeaveList()
        {
            try
            {
                var leaveList = await _employeeLeave.GetLeaveList();
                return leaveList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetLeaveRequestedDate")]

        public async Task<List<EmployeeLeaveDto>> GetLeaveRequestedDate(int? employeeId)
        {
            try
            {
                var employeeeList = await _employeeLeave.GetLeaveRequestedDate(employeeId);
                return employeeeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("UpdateLeaveRequest")]
        public async Task<bool> UpdateLeaveRequest(EmployeeLeaveDto employeeleavedto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeLeave.UpdateLeaveRequest(user, employeeleavedto);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetHolidayList")]

        public async Task<List<Day>> GetHolidayList(int year, int month)
        {
            try
            {
                var holidaylist = await _employeeLeave.GetHolidayList(year, month);
                return holidaylist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("UpdateDay")]

        public async Task<ActionResult<bool>> EditDay(Day day)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _employeeLeave.EditDay(day);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}