using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using System.Globalization;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.InteropServices;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public EmployeeService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<List<EmployeeDailyTask>> GetEmployeeHistory(DateTime fromDate, DateTime toDate)
        {
            try
            {
                //var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var employeeHistory = await _repository.DailyTask.FindByConditionAsync(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
                var result = employeeHistory.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateEmployee(UserCreateDto userCreateDto)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == userCreateDto.id);
                employee.Name = userCreateDto.UserName;
                employee.PhoneNumber = userCreateDto.PhoneNumber;
                employee.Department = userCreateDto.Department;
                employee.Category = userCreateDto.Category;
                employee.IsActive = userCreateDto.IsActive;
                employee.UpdatedDate = DateTime.Now;
                await _repository.Employee.UpdateAsync(employee);

                var user = await _repository.User.FindById(x => x.Id == userCreateDto.id);
                user.PhoneNumber = userCreateDto.PhoneNumber;
                user.Email = userCreateDto.Email;
                user.SecondaryEmail = userCreateDto.SecondaryEmail;
                user.SecondaryPhoneNumber = userCreateDto.SecondaryPhoneNumber;
                user.Name = userCreateDto.UserName;
                await _repository.User.UpdateAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<List<Employee>> GetEmployeeList()
        {
            try
            {
                var result = await _dbContext.Set<Employee>().Include(e => e.User).OrderByDescending(x => x.Id).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AssignSkill(EmployeeSkillSet[] empSkillSet)
        {
            try
            {
                var skillSet = _dbContext.EmployeeSkillSet.Where(x => x.EmployeeId == empSkillSet[0].EmployeeId).ToList();
                _dbContext.EmployeeSkillSet.RemoveRange(skillSet);

                foreach (var obj in empSkillSet)
                {

                    EmployeeSkillSet employeeSkillSet = new EmployeeSkillSet()
                    {
                        SkillSetId = obj.SkillSetId,
                        EmployeeId = obj.EmployeeId,
                        CreatedBy = obj.CreatedBy,
                        UpdatedBy = obj.UpdatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    await _dbContext.EmployeeSkillSet.AddAsync(employeeSkillSet);


                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<EmployeeSkillSet>> GetEmployeeSkillById(int employeeId)
        {

            try
            {
                var result = await _dbContext.EmployeeSkillSet.Where(x => x.EmployeeId == employeeId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<EmployeeStatDto> GetEmployeeStatDetails(User user)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var employeeProject = await _repository.EmployeeProject.GetEmployeeProjectList(employee.Id);
                var totalTask = await _repository.EmployeeTask.FindByConditionAsync(x => x.EmployeeId == employee.Id);
                var inProgressTask = await _repository.EmployeeTask.FindByConditionAsync(x => x.EmployeeId == employee.Id && x.Status == "In-Progress");
                var completedTask = await _repository.EmployeeTask.FindByConditionAsync(x => x.EmployeeId == employee.Id && (x.Status == "Completed" || x.Status == "Ready-For-UAT"));
                var result = inProgressTask.ToList();
                EmployeeStatDto empStat = new()
                {
                    TotalProject = employeeProject.Count(),
                    InProgressTask = inProgressTask.Count(),
                    TotalTask = totalTask.Count(),
                    CompletedTask = completedTask.Count()
                };
                return empStat;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TeamDto>> GetEmployeeTask(User sessionUser , int teamId, DateTime? weekend, int ProjectId, DateTime? date)
        {
            try
            {
                var query = await (from team in _dbContext.Team
                                   join teamEmployee in _dbContext.TeamEmployee on team.Id equals teamEmployee.TeamId
                                   join employee in _dbContext.Employee on teamEmployee.EmployeeId equals employee.Id
                                   join user in _dbContext.Users on employee.UserId equals user.Id
                                   where team.Id == teamId && teamEmployee.EndDate == null
                                   select new TeamDto
                                   {
                                       TeamId = team.Id,
                                       TeamName = team.Name,
                                       EmployeeId = teamEmployee.EmployeeId,
                                       EmployeeName = user.Name,
                                   }
                    ).Distinct().ToListAsync();


                DateTime weekEnd = weekend == null ? WeekEndingDate() : (DateTime)weekend;
                DateTime? weekStart = weekEnd.AddDays(-5);

                foreach (var obj in query)
                {
                    List<EmployeeTask> empTask = new List<EmployeeTask>();

                    if (ProjectId == 0)
                    {
                        empTask = _dbContext.EmployeeTask.Where(x => x.EmployeeId == obj.EmployeeId && x.WeekEndingDate.Date == weekEnd.Date && x.Status != "moved").ToList();
                    }
                    else
                    {
                        empTask = _dbContext.EmployeeTask.Where(x => x.EmployeeId == obj.EmployeeId && x.WeekEndingDate.Date == weekEnd.Date && x.ProjectId == ProjectId && x.Status != "moved").ToList();
                    }

                    if (sessionUser.UserType == "Customer")
                    {
                        var customer = _dbContext.Employee.FirstOrDefault(x => x.UserId == sessionUser.Id);
                        var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == customer.Id).Select(x => x.ProjectId).ToListAsync();
                        empTask = empTask.Where(x => customerProject.Any(cp => cp == x.ProjectId)).ToList();
                    }

                    decimal ActualHour = 0;
                    decimal EstimateHour = 0;
                    foreach (var emp in empTask)
                    {
                        if (date == null)
                        {
                            if (emp.WeekEndingDate.Date == weekEnd.Date)
                            {
                                ActualHour += emp.ActTime;
                                EstimateHour += emp.EstTime;
                            }
                        }
                        else
                        {
                            DateTime selectedDate = (DateTime)date;
                            if (emp.StartDate.Date == selectedDate.Date)
                            {
                                ActualHour += emp.ActTime;
                                EstimateHour += emp.EstTime;
                            }
                        }
                    }
                    obj.ActualHour = ActualHour;
                    obj.EstHour = EstimateHour;
                }
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<EachTeamMonthlyTask>> GetEmployeeMonthlyTask(int teamId)
        {
            try
            {

                DateTime currentDate = DateTime.Today;

                var team = _dbContext.Team.FirstOrDefault(x => x.Id == teamId);
                List<EachTeamMonthlyTask> eachTeamMonthlyTasks = new List<EachTeamMonthlyTask>();
                int month = DateTime.Now.Month;


                var teamtask = _dbContext.Task.Where(x => x.TeamId == teamId).ToList();
                for (int i = 1; i <= 12; i++)
                {
                    decimal ActualHour = 0;
                    decimal EstimateHour = 0;
                    foreach (var task in teamtask)
                    {
                        if (task.CreatedDate.HasValue && task.CreatedDate.Value.Month == i)
                        {
                            ActualHour += task.ActTime;
                            EstimateHour += task.EstTime;
                        }
                    }


                    EachTeamMonthlyTask eachTeamMonthlyTask = new EachTeamMonthlyTask()
                    {
                        ActualHour = ActualHour,
                        EstHour = EstimateHour,
                        Month = i,
                        TeamId = teamId,
                        TeamName = team.Name
                    };
                    eachTeamMonthlyTasks.Add(eachTeamMonthlyTask);

                }

                return eachTeamMonthlyTasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<EmployeeProject>> GetEmployeeProject(int projectId)
        {
            try
            {
                var result = await _dbContext.EmployeeProject.Where(x => x.ProjectId == projectId && x.EndDate == null).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<AttendanceDto> GetEmployeeAttendance(User sessionUser, DateTime selectedDate)
        {
            try
            {
                var day = _dbContext.Day.FirstOrDefault(x => x.Date.Date == selectedDate.Date);
                DateTime OfficeTime = new DateTime(1, 1, 1, 10, 10, 0);

                var TeamNameList = await (from teamEmployee in _dbContext.TeamEmployee
                                          join team in _dbContext.Team on teamEmployee.TeamId equals team.Id
                                          where teamEmployee.EndDate == null
                                          select new TeamMemeberList()
                                          {
                                              TeamId = teamEmployee.TeamId,
                                              EmployeeId = teamEmployee.EmployeeId,
                                              TeamName = team.Name
                                          }
                                ).ToListAsync();

                var TeamNameDistinctList = _dbContext.Team.ToList();
                var query = await (from employee in _dbContext.Employee
                                   join user in _dbContext.Users on employee.UserId equals user.Id into userGroup
                                   from user in userGroup.DefaultIfEmpty()
                                   select new AttendenceVm()
                                   {
                                       Id = user.Id,
                                       DayId = day.Id,
                                       EmployeeName = user.Name,
                                       Department = employee.Department,
                                       Date = day.Date,
                                       InTime = _dbContext.EmployeeTime.FirstOrDefault(x => x.DayId == day.Id && x.EmployeeId == employee.Id).InTime,
                                       OutTime = _dbContext.EmployeeTime.OrderBy(x => x.CreatedDate).LastOrDefault(x => x.DayId == day.Id && x.EmployeeId == employee.Id).OutTime,
                                       EmployeeTime = _dbContext.EmployeeTime.Where(x => x.DayId == day.Id && x.EmployeeId == employee.Id).ToList(),
                                       InOutCount = _dbContext.EmployeeTime.Where(x => x.DayId == day.Id && x.EmployeeId == employee.Id).Count(),
                                       EmployeeId = employee.Id
                                   }).OrderBy(x => x.EmployeeName).ToListAsync();

                var teamEmployees = await _dbContext.TeamEmployee.ToListAsync();
                var totalEmployees = _dbContext.Employee.Count();
                var presentEmployees = await _dbContext.EmployeeDay.Where(x => x.DayId == day.Id).ToListAsync();


                if (sessionUser.UserType == "Customer")
                {
                    var customer = _dbContext.Employee.FirstOrDefault(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == customer.Id).Select(x => x.ProjectId).ToListAsync();
                    if (customerProject.Count > 0)
                    {
                        var employeeProject = await _dbContext.EmployeeProject.Where(x => customerProject.Any(id => id == x.ProjectId)).ToListAsync();
                        query = query.Where(x => employeeProject.Any(ep => ep.EmployeeId == x.EmployeeId)).ToList();
                        totalEmployees = query.Count;
                        presentEmployees = presentEmployees.Where(x => employeeProject.Any(ep => ep.EmployeeId == x.EmployeeId)).ToList();
                    }
                }

                var absent = totalEmployees - presentEmployees.Count;
                int onTime = 0;
                DateTime date;

                foreach (var obj in query)
                {
                    var teamId = teamEmployees.FirstOrDefault(x => x.EmployeeId == obj.EmployeeId && x.EndDate == null)?.TeamId;
                    obj.TeamId = teamId;
                    if (obj.InTime != null)
                    {
                        date = (DateTime)obj.InTime;
                        if (date.TimeOfDay <= OfficeTime.TimeOfDay)
                        {
                            onTime++;
                        }
                    }
                }

                var attendence = new AttendanceDto()
                {
                    EmployeeCount = totalEmployees,
                    Present = presentEmployees.Count,
                    Absent = absent,
                    onTime = onTime,
                    Late = presentEmployees.Count - onTime,
                    Attendances = query
                };

                return attendence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmployeeAttendanceVM> GetAttendanceByUserId(int userId, int month, int year)
        {
            try
            {
                TimeSpan? averageInTime = null;
                TimeSpan? averageOutTime = null;
                var Day = await _dbContext.Day.ToListAsync();
                int dayId = Day.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date).Id;
                var employee = _dbContext.Employee.FirstOrDefault(x => x.UserId == userId);

                var attendanceHistory = (from day in _dbContext.Day
                                         where day.Date.Month == month && day.Date.Year == year && day.Id <= dayId
                                         select new EmployeeAttendanceDto()
                                         {
                                             Date = day.Date,
                                             InTime = _dbContext.EmployeeTime.FirstOrDefault(x => x.DayId == day.Id && x.EmployeeId == employee.Id).InTime,
                                             OutTime = _dbContext.EmployeeTime.Where(x => x.DayId == day.Id && x.EmployeeId == employee.Id).OrderBy(x => x.InTime).LastOrDefault().OutTime,
                                             EmployeeGeo = _dbContext.EmployeeGeo.Where(x => x.DayId == day.Id && x.EmployeeId == employee.Id).ToList()
                                         }).OrderByDescending(x => x.Date).ToList();

                var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
                int totalWorkDays = attendanceHistory.Count;
                int totalAttendance = attendanceHistory.Where(x => x.InTime != null).Count();
                int totalAbsent = totalWorkDays - totalAttendance;

                if (attendanceHistory.Count() > 0)
                {
                    int counter = attendanceHistory.Count() < 22 ? attendanceHistory.Count() : 22;
                    int total = 0;
                    TimeSpan inTime = new TimeSpan(0);
                    TimeSpan outTime = new TimeSpan(0);

                    for (int i = 0; i < counter; i++)
                    {
                        if (attendanceHistory[i].InTime != null && attendanceHistory[i].OutTime != null)
                        {
                            DateTime startDay = (DateTime)attendanceHistory[i].InTime;
                            DateTime endDay = (DateTime)attendanceHistory[i].OutTime;
                            inTime = inTime + startDay.TimeOfDay;
                            outTime = outTime + endDay.TimeOfDay;
                            total++;
                        }
                    }
                    averageInTime = total > 0 ? inTime / total : new TimeSpan(0);
                    averageOutTime = total > 0 ? outTime / total : new TimeSpan(0); ;
                }

                EmployeeAttendanceVM attendenceVm = new EmployeeAttendanceVM()
                {
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = "Employee",
                    TotalAttendance = totalAttendance,
                    TotalAbsent = totalAbsent,
                    AverageInTime = averageInTime?.ToString(@"hh\:mm"),
                    AverageOutTime = averageOutTime?.ToString(@"hh\:mm"),
                    EmployeeAttendances = attendanceHistory
                };

                return attendenceVm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmployeeTaskVM> GetEmployeeTasks(User sessionUser,int employeeId)
        {
            try
            {

                var employeeProjects = (from employeeProject in _dbContext.EmployeeProject
                                        join project in _dbContext.Project on employeeProject.ProjectId equals project.Id
                                        where employeeProject.EmployeeId == employeeId
                                        select new Project()
                                        {
                                            Id = project.Id,
                                            Name = project.Name
                                        }).ToList();

                var employeeTasks = (from employeeTask in _dbContext.EmployeeTask
                                     join task in _dbContext.Task on employeeTask.TaskId equals task.Id
                                     join user in _dbContext.Users on employeeTask.CreatedBy equals user.Id.ToString()
                                     join project in _dbContext.Project on task.ProjectId equals project.Id
                                     join userInterface in _dbContext.UserInterface on task.UserStoryUIId equals userInterface.Id into userInterfaceGroup
                                     from userInterface in userInterfaceGroup.DefaultIfEmpty()
                                     join userStory in _dbContext.UserStory on task.UserStoryId equals userStory.Id into userStoryGroup
                                     from userStory in userStoryGroup.DefaultIfEmpty()
                                     where employeeTask.EmployeeId == employeeId
                                     select new EmployeeTaskDto()
                                     {
                                         Id = employeeTask.Id,
                                         EmployeeId = employeeTask.EmployeeId,
                                         Description = task.Description,
                                         Category = _dbContext.Category.FirstOrDefault(x => x.Id == task.CategoryId).Categories,
                                         SubCategory = _dbContext.Category.FirstOrDefault(x => x.Id == task.CategoryId).SubCategory,
                                         Status = employeeTask.Status,
                                         StartDate = employeeTask.EstStartDate,
                                         EndDate = employeeTask.EstEndDate,
                                         WeekEndingDate = employeeTask.WeekEndingDate,
                                         CreatedBy = user.Name,
                                         ProjectId = employeeTask.ProjectId,
                                         EstTime = employeeTask.EstTime,
                                         TaskId = employeeTask.TaskId,
                                         ActTime = employeeTask.ActTime,
                                         Percentage = employeeTask.Percentage,
                                         Priority = employeeTask.Priority,
                                         UserInterface = userInterface.Name,
                                         UserStory = userStory.Description,
                                         ProjectName = project.Name
                                     }).OrderByDescending(x => x.Id).ToList();
                var TeamId = _dbContext.TeamEmployee.FirstOrDefault(x => x.EmployeeId == employeeId);

                if (sessionUser.UserType == "Customer")
                {
                    var customer = _dbContext.Employee.FirstOrDefault(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == customer.Id).Select(x => x.ProjectId).ToListAsync();
                    employeeProjects = employeeProjects.Where(x => customerProject.Any(cp => cp == x.Id)).ToList();
                    employeeTasks = employeeTasks.Where(x => customerProject.Any(cp => cp == x.ProjectId)).ToList();
                }

                EmployeeTaskVM employeeTaskVM = new EmployeeTaskVM()
                {
                    EmployeeDailyTask = employeeTasks,
                    EmployeeProjects = employeeProjects,
                    TeamId = TeamId?.TeamId
                };
                return employeeTaskVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private DateTime WeekEndingDate()
        {
            DateTime currentDate = DateTime.Today;
            if ((int)currentDate.DayOfWeek == 5) return currentDate;
            DateTime weekEndingDate;
            int daysUntilFriday = (int)DayOfWeek.Friday - (int)currentDate.DayOfWeek;
            if (daysUntilFriday <= 0)
                daysUntilFriday += 7;

            weekEndingDate = currentDate.AddDays(daysUntilFriday);
            weekEndingDate = weekEndingDate.Date.AddDays(1).AddTicks(-1);
            return weekEndingDate;
        }

        public async Task<Employee> UploadDocument(DocumentImageDto uploadDto)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.Id == uploadDto.EmployeeId);
                using (var memoryStream = new MemoryStream())
                {
                    uploadDto.File.CopyTo(memoryStream);
                    var ImageConvertToByte = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(ImageConvertToByte);

                    employee.ProfilePhoto = base64String;

                    var response = await _repository.Employee.UpdateAsync(employee);

                    return response;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Employee> DeleteDocument(DocumentImageDto uploadDto)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.Id == uploadDto.EmployeeId);
                using (var memoryStream = new MemoryStream())
                {
                    uploadDto.File.CopyTo(memoryStream);
                    var ImageConvertToByte = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(ImageConvertToByte);

                    employee.ProfilePhoto = base64String;

                    var response = await _repository.Employee.UpdateAsync(employee);

                    return response;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Task>> GetEmployeeTaskDetails(int employeeId, string status)
        {
            try
            {
                var result = from empTask in _dbContext.EmployeeTask
                             join task in _dbContext.Task on empTask.TaskId equals task.Id
                             where empTask.EmployeeId == employeeId
                             select new Task()
                             {
                                 Id = empTask.Id,
                                 Name = task.Name,
                                 Description = task.Description,
                                 Status = empTask.Status,
                                 Percentage = task.Percentage,
                                 TaskType = task.TaskType,
                             };

                if (status == "Task")
                {
                    result = result;
                }
                else if (status == "Completed")
                {
                    result = result.Where(empTask => empTask.Status == status || empTask.Status == "Ready-For-UAT");
                }
                else
                {
                    result = result.Where(empTask => empTask.Status == status);
                }

                var updatedResult = await result.ToListAsync();
                return updatedResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Task>> GetEmployeeTaskList(int Id, DateTime? weekend)
        {
            DateTime weekEnd = weekend == null ? WeekEndingDate() : (DateTime)weekend;
            DateTime? weekStart = weekEnd.AddDays(-5);

            var result = from empTask in _dbContext.EmployeeTask
                         join task in _dbContext.Task on empTask.TaskId equals task.Id
                         where empTask.EmployeeId == Id && empTask.WeekEndingDate.Date == weekEnd.Date
                         select new Task()
                         {
                             Id = empTask.Id,
                             Name = task.Name,
                             Description = task.Description,
                             Status = empTask.Status,
                             Percentage = task.Percentage,
                             EstTime = empTask.EstTime,
                         };
            var updatedResult = await result.ToListAsync();
            return updatedResult;
        }
        public async Task<List<Team>> GetEmployeeTeamList(User user)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var employeeTeams = await _dbContext.TeamEmployee
                     .Where(x => x.EmployeeId == employee.Id && x.EndDate == null)
                     .Include(te => te.Team)
                     .Select(te => te.Team)
                     .ToListAsync();
                return employeeTeams;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AttendanceDto> GetEmployeeAttendence(int Id, int? year, int? month)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            if (!month.HasValue)
            {
                month = DateTime.Now.Month;
            }

            var matchingDayIds = await _dbContext.Day
            .Where(d => d.Date.Year == year && d.Date.Month == month)
            .Select(d => d.Id)
            .ToListAsync();

            // Get employee attendance based on the matching DayIds
            var employeeAttendance = await _dbContext.EmployeeDay
                .Where(ed =>
                    ed.EmployeeId == Id &&
                    matchingDayIds.Contains(ed.DayId))
                .ToListAsync();


            var lateLogins = await _dbContext.EmployeeTime
               .Where(et =>
               et.EmployeeId == Id &&
               et.InTime.HasValue && et.InTime.Value.TimeOfDay > new TimeSpan(10, 0, 0) &&
               matchingDayIds.Contains((int)et.DayId))
               .ToListAsync();

            int absentCount = matchingDayIds.Count - employeeAttendance.Count;

            AttendanceDto attendance = new()
            {
                Present = employeeAttendance.Count,
                Absent = absentCount,
                Late = lateLogins.Count,
            };

            return attendance;
        }

        public async Task<List<AttendenceVm>> GetEmployeeLoginDetails(int Id, DateTime? weekend)
        {
            DateTime weekEnd = weekend == null ? WeekEndingDate() : (DateTime)weekend;
            DateTime? weekStart = weekEnd.AddDays(-5);

            List<AttendenceVm> loginDetails = new List<AttendenceVm>();


            var employeeLoginDetails = await _dbContext.EmployeeTime
                .Where(et => et.EmployeeId == Id && et.CreatedDate >= weekStart && et.CreatedDate <= weekEnd)
                .OrderBy(et => et.InTime)
                .ToListAsync();

            loginDetails = employeeLoginDetails.Select(et => new AttendenceVm
            {
                OutTime = et.OutTime,
                InTime = et.InTime,
                Date = et.CreatedDate,
            }).ToList();
            return loginDetails;
        }

        public async Task<List<TaskDTO>> GetEmployeeTasksId(AttendenceFilterDto TaskList)
        {
            int? totalCompletedCount = 0;
            int? totalInprogressCount = 0;
            bool isFilterApplied = false;
            var taskDetails = (from task in _dbContext.Task
                               join employeeTask in _dbContext.EmployeeTask on task.Id equals employeeTask.TaskId
                               where task.ProjectId == TaskList.projectId && employeeTask.EmployeeId == TaskList.employeeId
                               select new TaskDTO
                               {
                                   Id = task.Id,
                                   EmployeeId = employeeTask.EmployeeId,
                                   EmployeeTaskId = employeeTask.Id,
                                   Name = task.Name,
                                   Description = task.Description,
                                   Status = employeeTask.Status,
                                   EstimateStartDate = employeeTask.EstStartDate,
                                   EstimateEndDate = employeeTask.EstEndDate,
                                   EstTime = employeeTask.EstTime,
                                   WeekEndingDate = employeeTask.WeekEndingDate,
                               }).ToList();

            if (TaskList.months != null && TaskList.months.Count > 0)
            {
                isFilterApplied = true;
                taskDetails = taskDetails.Where(x => x.EmployeeId == TaskList.employeeId && x.EstimateStartDate.Month == TaskList.months[0]).ToList();

                if (TaskList.weekendingDate != null && TaskList.weekendingDate.Count > 0)
                {
                    List<TaskDTO> taskDto = new List<TaskDTO>();
                    foreach (var weekend in TaskList.weekendingDate)
                    {
                        DateTime weekendDate = DateTime.ParseExact(weekend, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var tasks = taskDetails
                            .Where(x => x.WeekEndingDate.HasValue && x.WeekEndingDate.Value.Date == weekendDate.Date)
                            .ToList();
                        taskDto.AddRange(tasks);
                    }
                    taskDetails = taskDto;
                }

                if (TaskList.status != null && TaskList.status.Count > 0)
                {
                    List<TaskDTO> taskDto = new List<TaskDTO>();
                    foreach (var status in TaskList.status)
                    {
                        var tasks = taskDetails.Where(x => x.Status == status).ToList();
                        taskDto.AddRange(tasks);
                    }
                    taskDetails = taskDto;
                }

            }

            if (TaskList.months != null && TaskList.months.Count == 0 && TaskList.status != null && TaskList.status.Count > 0)
            {
                isFilterApplied = true;
                List<TaskDTO> taskDto = new List<TaskDTO>();
                foreach (var status in TaskList.status)
                {
                    var tasks = taskDetails.Where(x => x.EmployeeId == TaskList.employeeId && x.Status == status && x.EstimateStartDate.Date == DateTime.Now.Date).ToList();
                    taskDto.AddRange(tasks);
                }
                taskDetails = taskDto;
            }

            if (!isFilterApplied)
            {
                taskDetails = taskDetails.Where(x => x.EmployeeId == TaskList.employeeId && x.EstimateStartDate.Date == DateTime.Now.Date).ToList();
            }


            var result = (from data in taskDetails
                          join comment in _dbContext.Comments on data.EmployeeTaskId equals comment.EmployeeTaskId into commentGroup
                          join employeedailytask in _dbContext.EmployeeDailyTask on data.EmployeeTaskId equals employeedailytask.EmployeeTaskId into dailyTaskGroup
                          select new TaskDTO()
                          {
                              Id = data.Id,
                              EmployeeTaskId = data.EmployeeTaskId,
                              ProjectId = data.ProjectId,
                              EmployeeId = data.EmployeeId,
                              Name = data.Name,
                              Description = data.Description,
                              Status = data.Status,
                              EstimateStartDate = data.EstimateStartDate,
                              EstimateEndDate = data.EstimateEndDate,
                              EstTime = data.EstTime,
                              Comments = commentGroup.Select(c => c.Comment).ToList(),
                              ActTime = dailyTaskGroup.LastOrDefault()?.ActTime,
                              ActualStartDate = dailyTaskGroup.LastOrDefault()?.StartDate,
                              ActualEndDate = dailyTaskGroup.LastOrDefault()?.EndDate,
                          }).ToList();


            totalCompletedCount = result.Count(employeeTask => employeeTask.Status == "Completed");
            totalInprogressCount = result.Count(employeeTask => employeeTask.Status == "In-Progress");

            foreach (var taskDto in result)
            {
                taskDto.CompletedTaskCount = totalCompletedCount;
                taskDto.InProgressTaskCount = totalInprogressCount;
            }

            return result;
        }

        public async Task<List<Employee>> GetCustomerList()
        {
            try
            {
                var result = await _dbContext.Set<Employee>()
                           .Include(e => e.User) 
                           .Where(e => e.User.UserType == "Customer")
                           .OrderByDescending(user => user.Id)
                           .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<CustomerProject>> GetCustomerProject(int projectId)
        {
            try
            {
                var result = await _dbContext.CustomerProject.Where(x => x.ProjectId == projectId && x.EndDate == null).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
