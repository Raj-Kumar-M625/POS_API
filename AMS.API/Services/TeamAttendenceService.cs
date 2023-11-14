using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;


namespace ProjectOversight.API.Services
{
    public class TeamAttendenceService : ITeamAttendenceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public TeamAttendenceService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

       
        public async Task<TeamDto> GetTeamDetailsById(int employeeId)
        {
            try
            {
                var result = (from employee in _dbContext.Employee
                              join tl in _dbContext.TeamLead on employee.Id equals tl.EmployeeId
                              join team in _dbContext.Team on tl.TeamId equals team.Id
                              where employee.Id == employeeId
                              select new TeamDto
                              {
                                  TeamName = team.Name,
                                  TeamId = team.Id,
                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<TeamAttendenceDto>> GetTeamLoginDetails(int teamId)
        {
            try
            {
                var indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var indianTime = TimeZoneInfo.ConvertTime(DateTime.Now, indianTimeZone);

                var day = _dbContext.Day.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date);
                var result = (from employee in _dbContext.Employee
                              join teamEmployee in _dbContext.TeamEmployee on employee.Id equals teamEmployee.EmployeeId
                              join user in _dbContext.Users on employee.UserId equals user.Id
                              where teamEmployee.EndDate == null && teamEmployee.TeamId == teamId
                              select new TeamAttendenceDto()
                              {
                                  Id = employee.Id,
                                  Name = user.Name,
                                  InTime = _dbContext.EmployeeTime.FirstOrDefault(x => x.EmployeeId == employee.Id && x.DayId == day.Id).InTime,
                                  OutTime = _dbContext.EmployeeTime.FirstOrDefault(x => x.EmployeeId == employee.Id && x.DayId == day.Id).OutTime,
                                  Comments = (from et in _dbContext.EmployeeTime
                                              join comment in _dbContext.Comments on et.Id equals comment.EmployeeTimeId
                                              where et.EmployeeId == employee.Id && et.DayId == day.Id
                                              select comment.Comment).FirstOrDefault()

                              }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TeamAttendanceStatDto> GetTeamAttendenceStatistics(int teamId)
        {
            try
            {
                var indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var indianTime = TimeZoneInfo.ConvertTime(DateTime.Now, indianTimeZone);
                TimeSpan? averageInTime = null;
                TimeSpan? averageOutTime = null;

                var attendanceData = (from tm in _dbContext.TeamEmployee
                                      join team in _dbContext.Team on tm.TeamId equals team.Id
                                      join employee in _dbContext.Employee on tm.EmployeeId equals employee.Id
                                      join et in _dbContext.EmployeeTime on tm.EmployeeId equals et.EmployeeId
                                      join comment in _dbContext.Comments on et.Id equals comment.EmployeeTimeId
                                      join u in _dbContext.User on employee.UserId equals u.Id
                                      where tm.TeamId == teamId && tm.EndDate == null && et.InTime >= indianTime.Date && et.InTime < indianTime.Date.AddDays(1)
                                      select new TeamAttendenceDto()
                                      {
                                          Id = et.Id,
                                          Name = u.Name,
                                          InTime = et.InTime,
                                          OutTime = et.OutTime,
                                          Comments = comment.Comment
                                      }).ToList();

                var presentCount = attendanceData.Count(a => a.InTime != null);
                var absentCount = attendanceData.Count(a => a.InTime == null);
                int lateCount = attendanceData.Count(a => a.InTime.Value.TimeOfDay > new TimeSpan(10, 0, 0));

                // Calculate the total InTime and OutTime as TimeSpans
                TimeSpan totalInTime = TimeSpan.Zero;
                TimeSpan totalOutTime = TimeSpan.Zero;
                int validInTimeCount = 0;
                int validOutTimeCount = 0;

                foreach (var attendance in attendanceData)
                {
                    if (attendance.InTime != null)
                    {
                        totalInTime += attendance.InTime.Value.TimeOfDay;
                        validInTimeCount++;
                    }

                    if (attendance.OutTime != null)
                    {
                        totalOutTime += attendance.OutTime.Value.TimeOfDay;
                        validOutTimeCount++;
                    }
                }

                // Calculate the average InTime and OutTime
                if (validInTimeCount > 0)
                {
                    averageInTime = TimeSpan.FromTicks(totalInTime.Ticks / validInTimeCount);
                }

                if (validOutTimeCount > 0)
                {
                    averageOutTime = TimeSpan.FromTicks(totalOutTime.Ticks / validOutTimeCount);
                }

                var summary = new TeamAttendanceStatDto()
                {
                    AvgInTime = averageInTime?.ToString(@"hh\:mm") ?? "N/A",
                    AvgOutTime = averageOutTime?.ToString(@"hh\:mm") ?? "N/A",
                    Present = presentCount,
                    Absent = absentCount,
                    Late = lateCount,
                    TeamAttendanceData = attendanceData
                };

                return summary;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<TeamAttendenceDto>> GetMemberAttendenceList(AttendenceFilterDto attendenceFilter)
        {
            DateTime currentDate = DateTime.Now;
            List<TeamAttendenceDto> loginDetails = new List<TeamAttendenceDto>();

           
            if (attendenceFilter.months == null || attendenceFilter.months.Count() == 0 || (attendenceFilter.months.Count() == 1 && attendenceFilter.months[0] == 0))
            {
                DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1); 
                DateTime endDate = startDate.AddMonths(1).AddDays(-1); 

                var attendanceData = (from day in _dbContext.Day
                                      join et in _dbContext.EmployeeTime on day.Id equals et.DayId
                                      join c in _dbContext.Comments on et.Id equals c.EmployeeTimeId
                                      where et.EmployeeId == attendenceFilter.employeeId && day.Date >= startDate && day.Date <= endDate
                                      select new TeamAttendenceDto()
                                      {
                                          OutTime = et.OutTime,
                                          InTime = et.InTime,
                                          Date = day.Date,
                                          Id = et.Id,
                                          Comments = c.Comment
                                      }).ToList();

                loginDetails.AddRange(attendanceData);
            }
            else
            {
                foreach (int month in attendenceFilter.months)
                {
                    DateTime startDate;

                    if (month == 0)
                    {
                        startDate = new DateTime(currentDate.Year, currentDate.Month, 1); // First day of the current month
                    }
                    else
                    {
                        startDate = new DateTime(currentDate.Year, month, 1); // First day of the specified month
                    }

                    DateTime endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the specified month

                    var attendanceData = (from day in _dbContext.Day
                                          join et in _dbContext.EmployeeTime on day.Id equals et.DayId
                                          join c in _dbContext.Comments on et.Id equals c.EmployeeTimeId
                                          where et.EmployeeId == attendenceFilter.employeeId && day.Date >= startDate && day.Date <= endDate
                                          select new TeamAttendenceDto()
                                          {
                                              OutTime = et.OutTime,
                                              InTime = et.InTime,
                                              Date = day.Date,
                                              Id = et.Id,
                                              Comments = c.Comment
                                          }).ToList();

                    loginDetails.AddRange(attendanceData);
                }
            }

            return loginDetails;
        }





        public async Task<TeamAttendanceStatDto> GetMemberAttendenceStat(int employeeId)
        {
            try
            {
                DateTime currentDate = DateTime.Now; // Current date
                DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1); // First day of the current month
                TimeSpan? averageInTime = null;
                TimeSpan? averageOutTime = null;

                var loginDetails = (from day in _dbContext.Day
                                    join et in _dbContext.EmployeeTime on day.Id equals et.DayId
                                    join c in _dbContext.Comments on et.Id equals c.EmployeeTimeId
                                    where et.EmployeeId == employeeId && day.Date >= startDate && day.Date <= currentDate
                                    select new TeamAttendenceDto()
                                    {
                                        Id = et.Id,
                                        OutTime = et.OutTime,
                                        InTime = et.InTime,
                                        Date = day.Date,
                                        Comments = c.Comment
                                    })
                                    .ToList();


                TimeSpan totalInTime = TimeSpan.Zero;
                TimeSpan totalOutTime = TimeSpan.Zero;
                int validInTimeCount = 0;
                int validOutTimeCount = 0;

                var presentCount = loginDetails.Count(a => a.InTime != null);
                var absentCount = loginDetails.Count(a => a.InTime == null);
                int lateCount = loginDetails.Count(a => a.InTime.Value.TimeOfDay > new TimeSpan(10, 0, 0));

                foreach (var attendance in loginDetails)
                {
                    if (attendance.InTime != null)
                    {
                        totalInTime += attendance.InTime.Value.TimeOfDay;
                        validInTimeCount++;
                    }

                    if (attendance.OutTime != null)
                    {
                        totalOutTime += attendance.OutTime.Value.TimeOfDay;
                        validOutTimeCount++;
                    }
                }

                // Calculate the average InTime and OutTime
                if (validInTimeCount > 0)
                {
                    averageInTime = TimeSpan.FromTicks(totalInTime.Ticks / validInTimeCount);
                }

                if (validOutTimeCount > 0)
                {
                    averageOutTime = TimeSpan.FromTicks(totalOutTime.Ticks / validOutTimeCount);
                }
                // Create a TeamAttendenceDto to hold the calculated values
                var summaryDto = new TeamAttendanceStatDto
                {
                    AvgInTime = averageInTime?.ToString(@"hh\:mm") ?? "N/A",
                    AvgOutTime = averageOutTime?.ToString(@"hh\:mm") ?? "N/A",
                    Present = presentCount,
                    Absent = absentCount,
                    Late = lateCount
                };

                return summaryDto;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
