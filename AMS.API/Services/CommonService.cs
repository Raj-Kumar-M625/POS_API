using AutoMapper;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Data;
using ProjectOversight.API.Services.Interface;
using ProjectOversight.API.Data.Model;
using Task = ProjectOversight.API.Data.Model.Task;
using Comment = ProjectOversight.API.Data.Model.Comments;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Constants;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Linq;

namespace ProjectOversight.API.Services
{
    public class CommonService : ICommonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly ProjectOversightContext _dbContext;
        public CommonService(IUnitOfWork repository, IMapper mapper, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _repository = repository;
            _dbContext = dbContext;
        }
        public async Task<List<Category>> GetCategoriesList()
        {
            try
            {
                var categoryList = await _repository.Category.FindAllAsync();
                var result = categoryList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Task>> GetProjectTaskList(int ProjectId)
        {
            try
            {
                var ProjectList = await _repository.Task.FindByConditionAsync(x => x.ProjectId == ProjectId);
                var result = ProjectList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddComment(User user, Comment comment)
        {
            try
            {
                Comment newComment = new Comment()
                {
                    Comment = comment.Comment,
                    TaskId = comment.TaskId,
                    ProjectId = comment.ProjectId,
                    EmployeeId = comment.EmployeeId,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                    UpdatedDate = DateTime.Now,
                };
                _dbContext.Comments.Add(newComment);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CommentsDto>> GetCommentsList(int TaskId)
        {
            try
            {
                var comments = await _repository.Comments.FindByConditionAsync(x => x.TaskId == TaskId);
                var employeeId = comments.Select(x => x.EmployeeId).ToList();
                var employees = await _repository.Employee.FindByConditionAsync(x => employeeId.Contains(x.Id));
                //var user = await _dbContext.Users.Where(x=>x.Id == employees.FirstOrDefault().UserId).FirstOrDefault();
                var userId = employees.Select(x => x.UserId).ToList();
                var users = await _repository.User.FindByConditionAsync(x => userId.Contains(x.Id));
                var task = await _repository.Task.FindByConditionAsync(x => x.Id == TaskId);

                var employeeTask = comments.Select(x => x.EmployeeTaskId).ToList();
                var employeesTaskId = await _repository.EmployeeTask.FindByConditionAsync(x => employeeTask.Contains(x.Id));

                var employeeDailyTask = comments.Select(x => x.EmployeeDailyTaskId).ToList();
                var employeesDailyTaskId = await _repository.DailyTask.FindByConditionAsync(x => employeeDailyTask.Contains(x.Id));
                var commentWithEmployeeNames = comments
         .Join(employees,
             comment => comment.EmployeeId,
             employee => employee.Id,
             (comment, employee) => new { comment, employee })
         .Join(users,
             joinResult => joinResult.employee.UserId,
             user => user.Id,
             (joinResult, user) => new CommentsDto
             {
                 ProjectId = joinResult.comment.ProjectId,
                 TaskId = joinResult.comment.TaskId,
                 EmployeeTaskId = joinResult.comment.EmployeeTaskId,
                 EmployeeDailyTaskId = joinResult.comment.EmployeeDailyTaskId,
                 EmployeeId = joinResult.comment.EmployeeId,
                 EmployeeTimeId = joinResult.comment.EmployeeTimeId,
                 Comment = joinResult.comment.Comment,
                 CreatedDate = joinResult.comment.CreatedDate,
                 EmployeeName = user.Name,
             }).ToList();

                foreach (var comment in commentWithEmployeeNames)
                {
                    if (comment.TaskId != null && comment.EmployeeDailyTaskId != null && comment.EmployeeTaskId != null)
                    {
                        comment.Status = employeesDailyTaskId.Where(x => x.Id == comment.EmployeeDailyTaskId).FirstOrDefault().Status;
                        comment.Percentage = employeesDailyTaskId.Where(x => x.Id == comment.EmployeeDailyTaskId).FirstOrDefault().Percentage;
                    }
                    else if (comment.TaskId != null && comment.EmployeeTaskId != null && comment.EmployeeDailyTaskId == null)
                    {
                        comment.Status = employeesTaskId.Where(x => x.Id == comment.EmployeeTaskId).FirstOrDefault().Status;
                        comment.Percentage = employeesTaskId.Where(x => x.Id == comment.EmployeeTaskId).FirstOrDefault().Percentage;
                    }
                    else if (comment.TaskId != null && comment.EmployeeTaskId == null && comment.EmployeeDailyTaskId == null)
                    {
                        comment.Status = task.Where(x => x.Id == TaskId).FirstOrDefault().Status;
                        comment.Percentage = task.Where(x => x.Id == TaskId).FirstOrDefault().Percentage;
                    }
                }
                return commentWithEmployeeNames;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Comments> AddReplyComments(User user, CommentsDto ComDetails)
        {
            try
            {
                if (ComDetails.EmployeeTaskId == null && ComDetails.TaskId != null && ComDetails.EmployeeDailyTaskId == null)
                {
                    Comments comment = new()
                    {
                        ProjectId = ComDetails.ProjectId,
                        TaskId = ComDetails.TaskId,
                        EmployeeId = ComDetails.EmployeeId,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString(),
                        Comment = ComDetails.Comment,
                    };
                    var addComments = await _repository.Comments.CreateAsync(comment);
                    return addComments;
                }
                else if (ComDetails.EmployeeDailyTaskId != null && ComDetails.EmployeeTaskId != null)
                {
                    Comments comment = new()
                    {
                        ProjectId = ComDetails.ProjectId,
                        TaskId = ComDetails.TaskId,
                        EmployeeId = ComDetails.EmployeeId,
                        EmployeeTaskId = ComDetails.EmployeeTaskId,
                        EmployeeDailyTaskId = ComDetails.EmployeeDailyTaskId,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString(),
                        Comment = ComDetails.Comment,
                    };
                    var addComments = await _repository.Comments.CreateAsync(comment);
                    return addComments;
                }
                else
                {
                    Comments comment = new()
                    {
                        ProjectId = ComDetails.ProjectId,
                        TaskId = ComDetails.TaskId,
                        EmployeeId = ComDetails.EmployeeId,
                        EmployeeTaskId = ComDetails.EmployeeTaskId,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString(),
                        Comment = ComDetails.Comment,
                    };
                    var addComments = await _repository.Comments.CreateAsync(comment);
                    return addComments;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DashboardDto> GetDashboardData(User sessionUser)
        {
            try
            {
                var day = _dbContext.Day.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date);
                DateTime OfficeTime = new DateTime(1, 1, 1, 10, 10, 0);

                var weekEnddate = WeekEndingDate();
                var empTask = _dbContext.EmployeeTask.Where(x => x.WeekEndingDate.Date == weekEnddate.Date).AsEnumerable();
                var projects = _dbContext.Project.ToList();

                var teamList = (from team in _dbContext.Team
                                join teamEmployee in _dbContext.TeamEmployee
                                on team.Id equals teamEmployee.TeamId into teamEmpJoin
                                from teamEmployee in teamEmpJoin.DefaultIfEmpty()
                                join employeeTask in empTask
                                on teamEmployee.EmployeeId equals employeeTask.EmployeeId into taskJoin
                                from employeeTask in taskJoin.DefaultIfEmpty()
                                where teamEmployee.EndDate == null
                                group new { employeeTask, team } by new { team.Name, team.Id } into g
                                select new TeamVM()
                                {
                                    Name = g.Key.Name,
                                    TeamId = g.Key.Id,
                                    AssignedHours = g.Sum(t => t.employeeTask.EstTime),
                                    UnAssignedHours = _dbContext.TeamEmployee.Where(x => x.TeamId == g.FirstOrDefault().team.Id && x.EndDate == null).Count() * 40 - g.Sum(t => t.employeeTask.EstTime),
                                }).OrderByDescending( t => t.UnAssignedHours).ToList();

                var TeamActiveWeekProject =(from te in _dbContext.TeamEmployee
                                            join et in _dbContext.EmployeeTask
                                            on te.EmployeeId equals et.EmployeeId 
                                            join e in _dbContext.Employee
                                            on et.EmployeeId equals e.Id
                                            join p in _dbContext.Project
                                            on et.ProjectId equals p.Id
                                            where te.EndDate == null && et.WeekEndingDate == weekEnddate.Date
                                            select new TeamWeeklyProjectItem()
                                            {
                                                TeamId = te.TeamId,
                                                ProjectName = p.Name,
                                                AssignedHours = et.EstTime
                                            }).ToList();

                var teamProjects = (from project in _dbContext.Project
                                    join teamProject in _dbContext.TeamProject on project.Id equals teamProject.ProjectId
                                    select new TeamProjectDto()
                                    {
                                        TeamId = teamProject.TeamId,
                                        ProjectName = project.Name,
                                        ProjectId = project.Id,
                                        Status = project.Status,
                                        Percentage = (int)project.Percentage,
                                    }).OrderByDescending(x => x.Percentage).ToList();

                var teamEmployees = (from employee in _dbContext.Employee
                                     join teamEmployee in _dbContext.TeamEmployee on employee.Id equals teamEmployee.EmployeeId
                                     join users in _dbContext.Users on employee.UserId equals users.Id
                                     where teamEmployee.EndDate == null
                                     select new TeamEmployeeDto()
                                     {
                                         TeamId = teamEmployee.TeamId,
                                         EmployeeId = employee.Id,
                                         EmployeeName = users.Name,
                                         AssignedHours = _dbContext.EmployeeTask.Where(x => x.EmployeeId == employee.Id && x.WeekEndingDate == WeekEndingDate().Date).Sum(x => x.EstTime),
                                         UnassignedHours = 40 - _dbContext.EmployeeTask.Where(x => x.EmployeeId == employee.Id && x.WeekEndingDate == WeekEndingDate().Date).Sum(x => x.EstTime),
                                     }).Distinct().ToList();

                var employeeTimes = (from employee in _dbContext.Employee
                                     join teamEmployee in _dbContext.TeamEmployee on employee.Id equals teamEmployee.EmployeeId
                                     join users in _dbContext.Users on employee.UserId equals users.Id
                                     where teamEmployee.EndDate == null
                                     select new EmployeeTimeDto()
                                     {
                                         TeamId = teamEmployee.TeamId,
                                         EmployeeId = employee.Id,
                                         EmployeeName = users.Name,
                                         InTime = _dbContext.EmployeeTime.FirstOrDefault(x => x.EmployeeId == employee.Id && x.DayId == day.Id).InTime,
                                         OutTime = _dbContext.EmployeeTime.FirstOrDefault(x => x.EmployeeId == employee.Id && x.DayId == day.Id).OutTime,
                                     }).ToList();

                if (sessionUser.UserType == "Customer")
                {
                    var customer = _dbContext.Employee.FirstOrDefault(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == customer.Id).Select(x => x.ProjectId).ToListAsync();
                    teamProjects = teamProjects.Where(x => customerProject.Any(id => id == x.ProjectId)).ToList();
                    employeeTimes = employeeTimes.Where(x => teamProjects.Any(tp => tp.TeamId == x.TeamId)).ToList();
                    teamList = teamList.Where(x => teamProjects.Any(tp => tp.TeamId == x.TeamId)).ToList();
                    teamEmployees = teamEmployees.Where(x => teamProjects.Any(tp => tp.TeamId == x.TeamId)).ToList();
                    TeamActiveWeekProject = TeamActiveWeekProject.Where(x => teamProjects.Any(tp => tp.TeamId == x.TeamId)).ToList();
                    projects = projects.Where(x => customerProject.Any(id => id == x.Id)).ToList();
                }

                var completedProject = projects.Where(x => x.Percentage == 100).Count();
                var onGoing = projects.Where(x => x.Percentage < 100 && x.Percentage > 0).Count();

                var dashboarDto = new DashboardDto()
                {
                    EmployeeTime = employeeTimes,
                    totalProject = projects.Count,
                    completedProject = completedProject,
                    onGoingProject = onGoing,
                    TeamList = teamList,
                    TeamEmployees = teamEmployees,
                    TeamWeeklyProjectItem = TeamActiveWeekProject,
                    TeamProjects = teamProjects
                };
                return dashboarDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TeamDashboardDto> GetTeamDashboardData(User sessionUser,int teamId)
        {
            try
            {
                var teamProject = await _dbContext.TeamProject.ToListAsync();
                var TeamDashboardList = (from team in _dbContext.Team
                                         join teamproject in _dbContext.TeamProject on team.Id equals teamproject.TeamId
                                         join project in _dbContext.Project on teamproject.ProjectId equals project.Id
                                         join teamMonthlyObjective in _dbContext.TeamMonthlyObjective on teamproject.TeamId equals teamMonthlyObjective.TeamId
                                         join teamWeeklyObjective in _dbContext.TeamWeeklyObjective on teamMonthlyObjective.Id equals teamWeeklyObjective.MonthlyObjectiveId
                                         where teamproject.TeamId == teamId && teamMonthlyObjective.ProjectId == project.Id
                                         select new TeamDashboardListDto
                                         {
                                             ProjectName = project.Name,
                                             TeamId = teamproject.TeamId,
                                             ProjectId = teamproject.ProjectId,
                                             Team = team.Name,
                                             MonthlyObjcectiveName = teamMonthlyObjective.Name,
                                             ObjectiveStatus = teamMonthlyObjective.Status,
                                             Objective = teamMonthlyObjective.Description,
                                             WeeklyObjectives = teamWeeklyObjective.Name,
                                             WeeklyObjectiveDescription = teamWeeklyObjective.Description,
                                             Percentage = project.Percentage,
                                             Type = project.Type,

                                         }).ToList();

                var totalteamObjectives = await _dbContext.TeamObjective.Where(x => x.TeamId == teamId).ToListAsync();

                DateTime currentDate = DateTime.Today;

                DateTime weekStartingDate = GetWeekStartingDate();
                DateTime weekEndingDate = WeekEndingDate();

                var TotalWeeklyObjective = new TeamDashboardListDto();

                var totalwEEKteamObjectives = await _dbContext.TeamWeeklyObjective.Where(x => x.TeamId == teamId && x.WeekEndingDate.Value.Date == weekEndingDate.Date ).ToListAsync();

                DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var MonthlyTeamObjectiveList = await _dbContext.TeamMonthlyObjective.Where(x => x.TeamId == teamId && x.CreatedDate.HasValue && x.CreatedDate.Value.Date >= firstDayOfMonth.Date && x.CreatedDate.HasValue && x.CreatedDate.Value.Date <= lastDayOfMonth.Date).ToListAsync();

                var TeamList = await (from team in _dbContext.Team
                                      join teamEmployee in _dbContext.TeamEmployee on team.Id equals teamEmployee.TeamId
                                      join employee in _dbContext.Employee on teamEmployee.EmployeeId equals employee.Id
                                      join user in _dbContext.Users on employee.UserId equals user.Id
                                      where team.Id == teamId && teamEmployee.EndDate == null
                                      select new TeamMemeberList
                                      {
                                          TeamId = team.Id,
                                          TeamName = team.Name,
                                          EmployeeId = employee.Id,
                                          EmployeeName = user.Name,
                                          AssignedHours = _dbContext.EmployeeTask
                                          .Where(x => x.EmployeeId == employee.Id && x.WeekEndingDate == WeekEndingDate().Date)
                                          .Sum(x => x.EstTime),
                                          UnassignedHours = 40 - _dbContext.EmployeeTask
                                           .Where(x => x.EmployeeId == employee.Id && x.WeekEndingDate == WeekEndingDate().Date)
                                          .Sum(x => x.EstTime),

            }).OrderBy(x => x.EmployeeName).ToListAsync();

               

                var totalAssignedHours = TeamList.Sum(member => member.AssignedHours);
                var totalUnassignedHours = TeamList.Sum(member => member.UnassignedHours);

                foreach (var member in TeamList)
                {
                    member.TotalAssignedHours = totalAssignedHours;
                    member.TotalUnassignedHours = totalUnassignedHours;
                }
                int currentMonth = currentDate.Month;

                foreach (var obj in TeamList)
                {
                    List<EmployeeTask> Memeberlist = new List<EmployeeTask>();

                    Memeberlist = _dbContext.EmployeeTask.Where(x => x.EmployeeId == obj.EmployeeId && x.CreatedDate.HasValue && x.CreatedDate.Value.Month == currentDate.Month).ToList();
                    decimal ActualHour = 0;

                    foreach (var emp in Memeberlist)
                    {
                        if (emp.WeekEndingDate.Month == currentDate.Month)
                        {
                            ActualHour += emp.ActTime;

                        }
                    }
                    obj.ActualHour = ActualHour;
                }

                var result = await (from Project in _dbContext.Project
                              join TeamProject in _dbContext.TeamProject on Project.Id equals TeamProject.ProjectId
                              where TeamProject.TeamId == teamId
                              select new TeamProjectList()
                              {
                                  Id = Project.Id,
                                  TeamId = TeamProject.TeamId,
                              }).ToListAsync();


                var TeamTechstack = (from projectTechstack in _dbContext.ProjectTechStack
                                     join projectcodevalue in _dbContext.CommonMaster on projectTechstack.TechStack equals projectcodevalue.Id
                                     join teamproject in _dbContext.TeamProject on projectTechstack.ProjectId equals teamproject.ProjectId
                                     where teamproject.TeamId == teamId
                                     select new ProjectTechStackDto
                                     {
                                         TechStackId = projectTechstack.TechStack,
                                         TechStackName = projectcodevalue.CodeValue
                                     }).Distinct().ToList();

                var EmployeeSkill = (from team in _dbContext.TeamEmployee
                                     join employeeskill in _dbContext.EmployeeSkillSet on team.EmployeeId equals employeeskill.EmployeeId
                                     join skill in _dbContext.SkillSet on employeeskill.SkillSetId equals skill.Id
                                     where team.TeamId == teamId && employeeskill.EmployeeId == team.EmployeeId
                                     select new SkillListDto
                                     {
                                         EmployeeId = employeeskill.EmployeeId,
                                         Skill = skill.Category
                                     }).Distinct().ToList();

                if (sessionUser.UserType == "Customer")
                {
                    var customer = _dbContext.Employee.FirstOrDefault(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == customer.Id).Select(x => x.ProjectId).ToListAsync();
                    TeamDashboardList = TeamDashboardList.Where(x => customerProject.Any(cp => cp == x.ProjectId)).ToList();
                    teamProject = teamProject.Where(x => customerProject.Any(cp => cp == x.ProjectId)).ToList();
                    MonthlyTeamObjectiveList = MonthlyTeamObjectiveList.Where(x => customerProject.Any(cp => cp == x.ProjectId)).ToList();
                    result = result.Where(x => customerProject.Any(cp => cp == x.Id)).ToList();
                }


                foreach (var obj in TeamList)
                {
                    obj.EmployeeSkills = EmployeeSkill.Where(x => x.EmployeeId == obj.EmployeeId).ToList(); ;
                }

                var projects = _dbContext.Project.ToList();
                var totalEmployees = _dbContext.Users.Count();
                var completedProject = projects.Where(x => x.Percentage == 100).Count();

                var teamDashboardDto = new TeamDashboardDto()
                {
                    totalTeamEmployees = TeamList.Count(),
                    TotalTeamProject = result.Count(),
                    TeamMember = TeamList,
                    ProjectTechstack = TeamTechstack,
                    totalteamObjectives = totalteamObjectives.Count(),
                    TeamDashboardListDtos = TeamDashboardList,
                    TotalWeekObjective = totalwEEKteamObjectives.Count(),
                    TotalMonthObjective = MonthlyTeamObjectiveList.Count(),
                };

                return teamDashboardDto;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<bool> TeamList()
        {
            var employeeTask = _dbContext.EmployeeTask.ToList();
            var teamEmployees = _dbContext.TeamEmployee.ToList();

            var weekEnddate = WeekEndingDate();
            var query = (from team in _dbContext.Team
                         join teamEmployee in _dbContext.TeamEmployee on team.Id equals teamEmployee.TeamId into teamGroup
                         from teamEmployee in teamGroup.DefaultIfEmpty()
                         select new TeamVM()
                         {
                             Name = team.Name,
                             AssignedHours = employeeTask.Where(x => x.EmployeeId == teamEmployee.EmployeeId && x.WeekEndingDate == weekEnddate).Sum(x => x.EstTime),
                             UnAssignedHours = (teamEmployees.Where(x => x.EmployeeId == teamEmployee.EmployeeId).Count() * 40) - employeeTask.Where(x => x.EmployeeId == teamEmployee.EmployeeId && x.WeekEndingDate == weekEnddate).Sum(x => x.EstTime),
                         }).ToList();

            return true;
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

        private static DateTime GetWeekStartingDate()
        {
            DateTime currentDate = DateTime.Today;

            DateTime weekEndingDate;
            int daysUntilMonday = (int)DayOfWeek.Monday - (int)currentDate.DayOfWeek;
            if (daysUntilMonday > 0)
                daysUntilMonday -= 7;

            return currentDate.AddDays(daysUntilMonday).Date;
        }
        public static List<DateTime> GetWeekendsStartingOnFriday(DateTime currentDate)
        {
            List<DateTime> weekends = new List<DateTime>();
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            DateTime currentDay = firstDayOfMonth;

            while (currentDay <= lastDayOfMonth)
            {
                if (currentDay.DayOfWeek == DayOfWeek.Friday && currentDay.AddDays(1).DayOfWeek == DayOfWeek.Saturday)
                {
                    weekends.Add(currentDay);
                }
                currentDay = currentDay.AddDays(1);
            }
            return weekends;
        }

        public async Task<int> UpdatePercentage()
        {
            try
            {
                var result = await _dbContext.Database.ExecuteSqlRawAsync("exec PercentageCalculation");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateOutTime()
        {
            try
            {
                var result = await _dbContext.Database.ExecuteSqlRawAsync("exec UpdateOutTime");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TaskTypeClassification>> GetTasktypeClassificationList()
        {
            try
            {
                var categoryList = await _repository.TaskTypeClassification.FindAllAsync();
                var result = categoryList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
       
 }
