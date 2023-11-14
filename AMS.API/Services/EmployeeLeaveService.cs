using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using System.Collections.Generic;

namespace ProjectOversight.API.Services
{
    public class EmployeeLeaveService : IEmployeeLeaveService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public EmployeeLeaveService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
           RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;

        }


        public async Task<bool> ApplyLeaveRequest(User user, EmployeeLeaveDto leaveDto)
        {
            try
            {
                var team = await _repository.TeamEmployeeRepository.FindByConditionAsync(x => x.EmployeeId == leaveDto.EmployeeId);
                EmployeeLeave leave = new();
                leave.EmployeeId = leaveDto.EmployeeId;
                leave.TeamId = team.FirstOrDefault().TeamId;
                leave.LeaveType = leaveDto.LeaveType;
                leave.LeaveSubType = leaveDto.LeaveSubType;
                leave.LeaveStatus = leaveDto.LeaveStatus;
                leave.LeaveReason = leaveDto.LeaveReason;
                leave.CreatedDate = DateTime.Now;
                leave.UpdatedDate = DateTime.Now;
                leave.CreatedBy = user.Id.ToString();
                leave.UpdatedBy = user.Id.ToString();

                var employeeLeave = await _repository.EmployeeLeave.CreateAsync(leave);

                foreach (var leaveRequestDate in leaveDto.LeaveRequestDate)
                {
                    EmployeeLeaveHistory employeeLeaveHistory = new();
                    employeeLeaveHistory.LeaveId = employeeLeave.Id;
                    employeeLeaveHistory.EmployeeId = employeeLeave.EmployeeId;
                    employeeLeaveHistory.LeaveRequestDate = leaveRequestDate.Date;
                    employeeLeaveHistory.ApproveStatus = leaveDto.LeaveStatus;
                    employeeLeaveHistory.CreatedDate = DateTime.Now;
                    employeeLeaveHistory.UpdatedDate = DateTime.Now;
                    employeeLeaveHistory.CreatedBy = user.Id.ToString();
                    employeeLeaveHistory.UpdatedBy = user.Id.ToString();

                    await _repository.EmployeeLeaveHistory.CreateAsync(employeeLeaveHistory);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LeaveHistoryDto>> GetAllEmployeeLeaveById(int employeeId, int selectedId)
        {
            var leaveHistory = from history in _dbContext.EmployeeLeaveHistory
                               join employeeLeave in _dbContext.EmployeeLeave
                               on history.LeaveId equals employeeLeave.Id
                               where employeeLeave.EmployeeId == employeeId && employeeLeave.Id == selectedId
                               select new LeaveHistoryDto
                               {
                                   Id = employeeLeave.Id,
                                   LeaveId = history.LeaveId,
                                   LeaveType = employeeLeave.LeaveType,
                                   LeaveSubType = employeeLeave.LeaveSubType,
                                   LeaveRequestDate = history.LeaveRequestDate,
                                   ApproveStatus = history.ApproveStatus,
                                   LeaveReason = employeeLeave.LeaveReason
                               };

            var leaveHistoryDtos = leaveHistory.ToList();
            return leaveHistoryDtos;
        }



        public async Task<List<EmployeeLeave>> GetAllEmployeeLeaveList(int employeeId)
        {
            try
            {
                var employeeeList = await _repository.EmployeeLeave.FindByConditionAsync(x => x.EmployeeId == employeeId);
                return employeeeList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<bool> EditLeaveRequest(User user, EmployeeLeaveDto leaveDto)
        {
            try
            {
                var leave = await _repository.EmployeeLeave.FindById(x => x.Id == leaveDto.Id);
                var team = await _repository.TeamEmployeeRepository.FindByConditionAsync(x => x.EmployeeId == leaveDto.EmployeeId);

                leave.TeamId = team.Count() > 0 ? team.FirstOrDefault().TeamId : 0;
                leave.LeaveType = leaveDto.LeaveType;
                leave.LeaveSubType = leaveDto.LeaveSubType;
                leave.LeaveStatus = leaveDto.LeaveStatus;
                leave.LeaveReason = leaveDto.LeaveReason;
                leave.UpdatedDate = DateTime.Now;
                leave.UpdatedBy = user.Id.ToString();

                var employee = await _repository.Employee.FindById(x => x.Id == leaveDto.EmployeeId);
                if (employee != null)
                {
                    var employeeName = employee.Name;
                }
                var employeeLeave = await _repository.EmployeeLeave.UpdateAsync(leave);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EmployeeLeaveDto>> GetLeaveByTeamId(int teamid)
        {
            try
            {
                var employeeeList = await _repository.EmployeeLeave.FindByConditionAsync(x => x.TeamId == teamid);
                var leaveList = _mapper.Map<IEnumerable<EmployeeLeaveDto>>(employeeeList).ToList();
                var employeeLeaveHistory = await _dbContext.EmployeeLeaveHistory.ToListAsync();
                var employee = await _dbContext.Employee.ToListAsync();

                foreach (var employeeLeave in leaveList)
                {
                    employeeLeave.LeaveHistory = employeeLeaveHistory.Where(x => x.LeaveId == employeeLeave.Id).ToList();
                    employeeLeave.EmployeeName = employee.FirstOrDefault(x => x.Id == employeeLeave.EmployeeId).Name;
                }
                return leaveList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<LeaveHistoryDto>> GetLeaveList()
        {
            try
            {
                var leaveList = (from employeeLeave in _dbContext.EmployeeLeave
                                 where employeeLeave.LeaveStatus == "pending"
                                 group employeeLeave by employeeLeave.TeamId into teamGroup
                                 select new LeaveHistoryDto
                                 {
                                     TeamId = teamGroup.Key,
                                     LeaveStatusCount = teamGroup.Count()
                                 }).ToList();

                return leaveList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EmployeeLeaveDto>> GetLeaveRequestedDate(int? employeeId)
        {
            try
            {
                var employeeeList = await _repository.EmployeeLeave.FindByConditionAsync(x => x.EmployeeId == employeeId);
                var leaveList = _mapper.Map<IEnumerable<EmployeeLeaveDto>>(employeeeList).ToList();
                var employeeLeaveHistory = await _dbContext.EmployeeLeaveHistory.ToListAsync();

                foreach (var employeeLeave in leaveList)
                {
                    employeeLeave.LeaveHistory = employeeLeaveHistory.Where(x => x.LeaveId == employeeLeave.Id).ToList();
                }
                return leaveList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateLeaveRequest(User user, EmployeeLeaveDto employeeleavedto)
        {
            try
            {
                var employeeleave = _dbContext.EmployeeLeave.FirstOrDefault(x => x.Id == employeeleavedto.Id);
                if (employeeleave != null)
                {
                    employeeleave.LeaveStatus = employeeleavedto.LeaveStatus;
                    _dbContext.EmployeeLeave.Update(employeeleave);
                }

                foreach (var obj in employeeleavedto.LeaveHistory)
                {
                    var leavehistory = _dbContext.EmployeeLeaveHistory.FirstOrDefault(x => x.Id == obj.Id);
                    if (leavehistory != null)
                    {
                        leavehistory.ApproveStatus = obj.ApproveStatus;
                        leavehistory.ApprovedBy = user.Id.ToString();
                        leavehistory.UpdatedBy = user.Id.ToString();
                        leavehistory.RejectedReason = obj.RejectedReason;
                        _dbContext.EmployeeLeaveHistory.Update(leavehistory);
                    }
                }
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Day>> GetHolidayList(int year, int month)
        {
            try
            {

                var currentDate = DateTime.Now;

                var HolidayList = await _dbContext.Day
                    .Where(x => x.Date.Year == currentDate.Year && x.Date.Month == month)
                    .ToListAsync();
                return HolidayList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> EditDay(Day day)
        {
            try
            {

                var HolidayList = _dbContext.Day.FirstOrDefault(x => x.Id == day.Id);

                if (HolidayList != null)
                {
                    HolidayList.HolidayName = day.HolidayName;
                    HolidayList.HolidayApplicable = day.HolidayApplicable;
                    _dbContext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
            