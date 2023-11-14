using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Constants;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using System.Threading.Tasks;
using System.Xml.Linq;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services
{
    public class TaskService : ITaskService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly ProjectOversightContext _dbContext;
        public TaskService(IUnitOfWork repository, IMapper mapper, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _repository = repository;
            _dbContext = dbContext;
        }
        public async Task<List<TaskListDto>> GetTaskList(TaskFilter filter,int month,int year)
        {
            try
            {
                var taskList = (from task in _dbContext.Task
                                join project in _dbContext.Project on task.ProjectId equals project.Id into projectJoin
                                from project in projectJoin.DefaultIfEmpty()
                                join employeeTask in _dbContext.EmployeeTask on task.Id equals employeeTask.TaskId into employeeTaskJoin
                                from employeeTask in employeeTaskJoin.DefaultIfEmpty()
                                join team in _dbContext.Team on task.TeamId equals team.Id into teamJoin
                                from team in teamJoin.DefaultIfEmpty()
                                join employee in _dbContext.Employee on employeeTask.EmployeeId equals employee.Id into employeeJoin
                                from employee in employeeJoin.DefaultIfEmpty()
                                where employeeTask.Status != "moved" && employeeTask.Status != "reassigned"
                                join userStoryUI in _dbContext.UserStoryUI on task.UserStoryUIId equals userStoryUI.Id into userStoryUIJoin
                                from userStoryUI in userStoryUIJoin.DefaultIfEmpty()
                                join userStory in _dbContext.UserStory on userStoryUI.UserStoryId equals userStory.Id into userStoryJoin
                                from userStory in userStoryJoin.DefaultIfEmpty()
                                join userInterface in _dbContext.UserInterface on userStoryUI.UIId equals userInterface.Id into userInterfaceJoin
                                from userInterface in userInterfaceJoin.DefaultIfEmpty()
                                select new TaskListDto
                                {
                                    Id = task.Id,
                                    ProjectId = task.ProjectId,
                                    Name = task.Name,
                                    projectName = project.Name,
                                    Description = task.Description,
                                    UIName = userInterface.Name,
                                    USName = task.UserStoryUIId == 0 ? _dbContext.UserStory.Where(x => x.Id == task.UserStoryId).Select(x => x.Name).FirstOrDefault() : userStory.Name,
                                    TeamName = team.Name,
                                    EmployeeName = employee.Name,
                                    Category = task.Category.Categories,
                                    SubCategory = task.Category.SubCategory,
                                    Status = task.Status,
                                    Percentage = task.Percentage,
                                    EstimateTime = task.EstTime,
                                    ActualTime = task.ActTime,
                                    UserStoryId = task.UserStoryId,
                                    UserStoryUIId = task.UserStoryUIId,
                                    TeamId = task.TeamId,
                                    EstimateStartDate = task.EstimateStartDate,
                                    EstimateEndDate = task.EstimateEndDate,
                                    ActualStartDate = task.ActualStartDate,
                                    ActualEndDate = task.ActualEndDate,
                                    weekEndDate = task.WeekEndingDate,
                                    TaskType = task.TaskType,
                                    Priority = task.Priority
                                }).OrderByDescending(x => x.Id).ToList();

                bool isFilterApplied = false;

                if (filter.EstStartDate != null && filter.EstEndDate != null)
                {
                    isFilterApplied = true;
                    taskList = taskList.Where(x => x.EstimateStartDate >= filter.EstStartDate 
                                              && x.EstimateEndDate <= filter.EstEndDate).ToList();
                }

                if (filter.ActStartDate != null && filter.ActEndDate != null)
                {
                    isFilterApplied = true;
                    taskList = taskList.Where(x => x.ActualStartDate >= filter.ActStartDate
                                              && x.ActualEndDate <= filter.ActEndDate).ToList();
                }

                if (!isFilterApplied)
                {
                    //taskList = taskList.Where(x => x.EstimateStartDate.HasValue && x.EstimateStartDate.Value.Month ==
                    //                               month && x.EstimateStartDate.Value.Year == year).ToList();
                }

                if(filter.WeekEndingDate != null)
                {
                    taskList = taskList.Where(x => x.weekEndDate.HasValue && x.weekEndDate.Value.Date 
                                                == filter.WeekEndingDate.Value.Date).ToList();
                }

                if (filter.ProjectName != null)
                {
                    taskList = taskList.Where(x => x.projectName.Trim() == filter.ProjectName.Trim()).ToList();
                }

                if(filter.TeamName != null)
                {
                    taskList = taskList.Where(x => x.TeamName.Trim() == filter.TeamName.Trim()).ToList();
                }

                if(filter.EmployeeName != null)
                {
                    taskList = taskList.Where(x => x.EmployeeName != null && x.EmployeeName.Trim() == filter.EmployeeName.Trim()).ToList();
                }


                if (filter.Priority != null)
                {
                    taskList = taskList.Where(x => x.Priority.Trim() == filter.Priority.Trim()).ToList();
                }

                if (filter.Category != null)
                {
                    taskList = taskList.Where(x => x.Category.Trim() == filter.Category.Trim()).ToList();
                }

                if (filter.SubCategory != null)
                {
                    taskList = taskList.Where(x => x.SubCategory.Trim() == filter.SubCategory.Trim()).ToList();
                }

                if (filter.Status != null)
                {
                    taskList = taskList.Where(x => x.Status.Trim() == filter.Status.Trim()).ToList();
                }

                if (filter.Percentage != null)
                {
                    taskList = taskList.Where(x => x.Percentage == filter.Percentage).ToList();
                }

                if (filter.ActualTime != null)
                {
                    taskList = taskList.Where(x => x.ActualTime == filter.ActualTime).ToList();
                }

                if (filter.EstimatedTime != null)
                {
                    taskList = taskList.Where(x => x.EstimateTime == filter.EstimatedTime).ToList();
                }
                return taskList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<Task> CreateTask(User user, TaskDTO task)
        {
            try
            {
                UserStoryUI UserStoryUi = new();
                var UserStoryUiId = await _repository.UserStoryUI.FindByConditionAsync(x => x.UserStoryId == task.UserStoryId && x.UIId == task.UIId);
                if (UserStoryUiId.Any())
                {
                    UserStoryUi = UserStoryUiId.First();
                }
                DateTime currentDate = DateTime.Today;

                var teamId = await _dbContext.TeamProject.Where(x => x.ProjectId == task.ProjectId).FirstAsync();

                var EmployeeProjectTask = _mapper.Map<Task>(task);
                if (UserStoryUi != null)
                {
                    EmployeeProjectTask.UserStoryUIId = UserStoryUi.Id;
                }

                EmployeeProjectTask.CreatedBy = user.Id.ToString();
                EmployeeProjectTask.UpdatedBy = user.Id.ToString();
                EmployeeProjectTask.CreatedDate = DateTime.Now;
                EmployeeProjectTask.UpdatedDate = DateTime.Now;
                EmployeeProjectTask.TeamId = teamId.TeamId;
                var taskcreated = await _repository.Task.CreateAsync(EmployeeProjectTask);
                Comments comment = new()
                {
                    ProjectId = task.ProjectId,
                    TaskId = taskcreated.Id,
                    EmployeeId = task.EmployeeId,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                    Comment = task.Comment,
                };
                var addComments = await _repository.Comments.CreateAsync(comment);

                return taskcreated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<UserInterface>> GetUserInterfacelist(int UserStoryId)
        {
            try
            {
                var USUIList = await _repository.UserStoryUI.GetUIlist(UserStoryId);
                var UIList = await _repository.UserInterface.FindByConditionAsync(app => USUIList.Contains(app.Id));
                var result = UIList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<UserStory>> GetProjectUSlist(int ProjectId)
        {
            try
            {
                var USList = await _repository.UserStory.FindByConditionAsync(x => x.ProjectId == ProjectId);
                var result = USList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TaskListDto>> GetEmployeeTaskList(TaskFilter filter,int employeeId)
        {
            try
            {
                DateTime date = DateTime.Now.AddDays(-60).Date;
                var taskList = (from task in _dbContext.Task
                                join project in _dbContext.Project on task.ProjectId equals project.Id into projectJoin
                                from project in projectJoin.DefaultIfEmpty()
                                join employeeTask in _dbContext.EmployeeTask on task.Id equals employeeTask.TaskId into employeeTaskJoin
                                from employeeTask in employeeTaskJoin.DefaultIfEmpty()
                                join team in _dbContext.Team on task.TeamId equals team.Id into teamJoin
                                from team in teamJoin.DefaultIfEmpty()
                                join user in _dbContext.Users on task.CreatedBy equals user.Id.ToString() into userJoin
                                from user in userJoin.DefaultIfEmpty()
                                join employee in _dbContext.Employee on user.Id equals employee.UserId into employeeJoin
                                from employee in employeeJoin.DefaultIfEmpty()
                                join userStoryUI in _dbContext.UserStoryUI on task.UserStoryUIId equals userStoryUI.Id into userStoryUIJoin
                                from userStoryUI in userStoryUIJoin.DefaultIfEmpty()
                                join userStory in _dbContext.UserStory on userStoryUI.UserStoryId equals userStory.Id into userStoryJoin
                                from userStory in userStoryJoin.DefaultIfEmpty()
                                join userInterface in _dbContext.UserInterface on userStoryUI.UIId equals userInterface.Id into userInterfaceJoin
                                from userInterface in userInterfaceJoin.DefaultIfEmpty()
                                where employee.Id == employeeId && task.CreatedDate.HasValue && task.CreatedDate.Value.Date >= date
                                select new TaskListDto()
                                {
                                    Id = task.Id,
                                    Name = task.Name,
                                    EmployeeTaskId = employeeTask.Id,
                                    ProjectId = task.ProjectId,
                                    projectName = project.Name,
                                    Description = task.Description,
                                    TeamName = team.Name,
                                    EmployeeName = user.Name,
                                    Category = task.Category.Categories,
                                    SubCategory = task.Category.SubCategory,
                                    Status = task.Status,
                                    UIName = userInterface.Name,
                                    USName = task.UserStoryUIId == 0 ? _dbContext.UserStory.Where(x => x.Id == task.UserStoryId).Select(x => x.Name).FirstOrDefault() : userStory.Name,
                                    Percentage = task.Percentage,
                                    EstimateTime = task.EstTime,
                                    ActualTime = task.ActTime,
                                    EstimateStartDate = task.EstimateStartDate,
                                    EstimateEndDate = task.EstimateEndDate,
                                    ActualStartDate = task.ActualStartDate,
                                    ActualEndDate = task.ActualEndDate,
                                    weekEndDate = task.WeekEndingDate,
                                    Priority = task.Priority
                                }).OrderByDescending(x => x.Id).ToList();
                bool isFilterApplied = false;

                if (filter.EstStartDate != null && filter.EstEndDate != null)
                {
                    isFilterApplied = true;
                    taskList = taskList.Where(x => x.EstimateStartDate >= filter.EstStartDate
                                              && x.EstimateEndDate <= filter.EstEndDate).ToList();
                }

                if (filter.ActStartDate != null && filter.ActEndDate != null)
                {
                    isFilterApplied = true;
                    taskList = taskList.Where(x => x.ActualStartDate >= filter.ActStartDate
                                              && x.ActualEndDate <= filter.ActEndDate).ToList();
                }

                if (!isFilterApplied)
                {
                    //taskList = taskList.Where(x => x.EstimateStartDate.HasValue && x.EstimateStartDate.Value.Month ==
                    //                               month && x.EstimateStartDate.Value.Year == year).ToList();
                }

                if (filter.WeekEndingDate != null)
                {
                    taskList = taskList.Where(x => x.weekEndDate.HasValue && x.weekEndDate.Value.Date
                                                == filter.WeekEndingDate.Value.Date).ToList();
                }

                if (filter.ProjectName != null)
                {
                    taskList = taskList.Where(x => x.projectName.Trim() == filter.ProjectName.Trim()).ToList();
                }

                if (filter.TeamName != null)
                {
                    taskList = taskList.Where(x => x.TeamName.Trim() == filter.TeamName.Trim()).ToList();
                }

                if (filter.EmployeeName != null)
                {
                    taskList = taskList.Where(x => x.EmployeeName != null && x.EmployeeName.Trim() == filter.EmployeeName.Trim()).ToList();
                }


                if (filter.Priority != null)
                {
                    taskList = taskList.Where(x => x.Priority.Trim() == filter.Priority.Trim()).ToList();
                }

                if (filter.Category != null)
                {
                    taskList = taskList.Where(x => x.Category.Trim() == filter.Category.Trim()).ToList();
                }

                if (filter.SubCategory != null)
                {
                    taskList = taskList.Where(x => x.SubCategory.Trim() == filter.SubCategory.Trim()).ToList();
                }

                if (filter.Status != null)
                {
                    taskList = taskList.Where(x => x.Status.Trim() == filter.Status.Trim()).ToList();
                }

                if (filter.Percentage != null)
                {
                    taskList = taskList.Where(x => x.Percentage == filter.Percentage).ToList();
                }

                if (filter.ActualTime != null)
                {
                    taskList = taskList.Where(x => x.ActualTime == filter.ActualTime).ToList();
                }

                if (filter.EstimatedTime != null)
                {
                    taskList = taskList.Where(x => x.EstimateTime == filter.EstimatedTime).ToList();
                }

                return taskList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Task>> GetTaskListValue()
        {
            try
            {
                var taskList = _dbContext.Task.Include(o => o.Project).Distinct().OrderByDescending(x => x.Id).ToList();
                return taskList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CustomerProject>> GetCustomerProject(int Id)
        {
            try
            {
                var EmployeeId = _dbContext.Employee.Where(x => x.UserId == Id).FirstOrDefault();
                var customerList = _dbContext.CustomerProject.Where(x => x.EmployeeId == EmployeeId.Id) .ToList();
                return customerList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }        

        public async Task<List<CommentsDto>> GetComments(int taskId)
        {
            try
            {
                var commentList = (from comment in _dbContext.Comments
                                   join employee in _dbContext.Employee on comment.EmployeeId equals employee.Id
                                   join user in _dbContext.Users on employee.UserId equals user.Id
                                   where comment.TaskId == taskId
                                   select new CommentsDto()
                                   {
                                       Id = comment.Id,
                                       Comment = comment.Comment,
                                       Employee = user.Name,
                                       EmployeeTimeId = comment.EmployeeTimeId,
                                       CreatedOn = comment.CreatedDate,
                                       TaskId = comment.TaskId,
                                       ProjectId = comment.ProjectId
                                   }).OrderByDescending(x => x.Id).ToList();
                return commentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Task> CreateTaskCheckList(User user, TaskCheckListDto CheckList)
        {
            try
            {
                UserStoryUI UserStoryUi = new();
                var UserStoryUiId = await _repository.UserStoryUI.FindByConditionAsync(x => x.UserStoryId == CheckList.UserStoryId && x.UIId == CheckList.UIId);
                if (UserStoryUiId.Any())
                {
                    UserStoryUi = UserStoryUiId.First();
                }
                DateTime currentDate = DateTime.Today;

                var teamId = await _dbContext.TeamProject.Where(x => x.ProjectId == CheckList.ProjectId).FirstAsync();

                var EmployeeProjectTask = _mapper.Map<Task>(CheckList);
                if (UserStoryUi != null)
                {
                    EmployeeProjectTask.UserStoryUIId = UserStoryUi.Id;
                }

                EmployeeProjectTask.CreatedBy = user.Id.ToString();
                EmployeeProjectTask.UpdatedBy = user.Id.ToString();
                EmployeeProjectTask.CreatedDate = DateTime.Now;
                EmployeeProjectTask.UpdatedDate = DateTime.Now;
                EmployeeProjectTask.TeamId = teamId.TeamId;
                var taskcreated = await _repository.Task.CreateAsync(EmployeeProjectTask);

                Comments comment = new()
                {
                    ProjectId = CheckList.ProjectId,
                    TaskId = taskcreated.Id,
                    EmployeeId = CheckList.EmployeeId,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                    Comment = CheckList.Comment,
                };
                var addComments = await _repository.Comments.CreateAsync(comment);

                foreach (var description in CheckList.CheckListDescriptions)
                {
                    TaskCheckList taskCheckListDescription = new()
                    {
                        TaskId = taskcreated.Id,
                        ProjectId = CheckList.ProjectId,
                        USId = CheckList.UserStoryId,
                        CategoryId = CheckList.CategoryId,
                        UIId = CheckList.UIId,
                        CheckListDescription = description,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString(),
                        UserStoryUIId = UserStoryUi.Id,

                    };
                    var addCheckList = await _repository.TaskCheckList.CreateAsync(taskCheckListDescription);
                }
                return taskcreated;


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> UpdateTask(TaskDTO task,User user)
        {
            try
            {
                var taskFromdb = _dbContext.Task.FirstOrDefault(x => x.Id == task.Id);

                if (taskFromdb != null)
                {
                    taskFromdb.Name = task.Name;
                    taskFromdb.Description = task.Description;
                    taskFromdb.CategoryId = task.CategoryId;
                    taskFromdb.EstimateStartDate = task.EstimateStartDate;
                    taskFromdb.EstimateEndDate = task.EstimateEndDate;
                    taskFromdb.Status = task.Status;
                    taskFromdb.UpdatedDate = DateTime.Now;
                    taskFromdb.UpdatedBy = user.Id.ToString();

                    _dbContext.Task.Update(taskFromdb);
                    _dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProjectTask> getProjectTaskList(int projectId)
        {
            try
            {
                var OverTaskList = (from task in _dbContext.Task
                                    join employeetask in _dbContext.EmployeeTask on task.Id equals employeetask.TaskId
                                    join employee in _dbContext.Employee on employeetask.EmployeeId equals employee.Id
                                    join user in _dbContext.Users on employee.UserId equals user.Id
                                    where task.ProjectId == projectId 
                                    select new ProjectTaskListDto()
                                    {
                                        Id = task.Id,
                                        EmployeeId = employee.Id,
                                        EmployeeName = user.Name,
                                        Names = task.Name,
                                        Description = task.Description,
                                        Status = employeetask.Status,
                                        percentage = task.Percentage,
                                        estTime = employeetask.ActTime,
                                        actTime = employeetask.EstTime,
                                        EstimatedStartedDate = task.EstimateStartDate,
                                        EstimatedEndedDate = task.EstimateEndDate,
                                    }
                                     ).ToList();


                var totalTask = await _repository.Task.FindByConditionAsync(x => x.ProjectId == projectId);
                var OnGoing = _dbContext.Task.Where(x => x.ProjectId == projectId && x.Status == "In-Progress").Count();
                var Completed = _dbContext.Task.Where(X => X.ProjectId == projectId && X.Status == "Completed").Count();
                var Assigned = _dbContext.Task.Where(x => x.ProjectId == projectId && x.Status == "Assigned").Count();
                var ReadyForUAT = _dbContext.Task.Where(x => x.ProjectId == projectId && x.Status == "Ready-For-UAT").Count();
                var Unassigned = _dbContext.Task.Where(v => v.ProjectId == projectId && v.Status == "Unassigned").Count();

                DateTime currentDate = DateTime.Today;

                DateTime weekStartingDate = GetWeekStartingDate();
                DateTime weekEndingDate = WeekEndingDate();

                var createWeek = (from task in _dbContext.Task
                                  join employetask in _dbContext.EmployeeTask on task.Id equals employetask.TaskId 
                                  join user in _dbContext.Users on employetask.EmployeeId equals user.Id
                                  where task.ProjectId == projectId && task.CreatedDate.HasValue && task.CreatedDate.Value.Date >= weekStartingDate.Date && task.CreatedDate.HasValue && task.CreatedDate.Value.Date <= weekEndingDate.Date
                                  select new WeeklyTaskList()
                                  {
                                      Id = task.Id,
                                      EmployeeName = user.Name,
                                      Names = task.Name,
                                      Description = task.Description,
                                      Status = employetask.Status,
                                      percentage = task.Percentage,
                                      estTime = employetask.ActTime,
                                      actTime = employetask.EstTime,
                                      EstimatedStartedDate = task.EstimateStartDate,
                                      EstimatedEndedDate = task.EstimateEndDate,
                                  }).ToList();

                List<DateTime> weekendsStartingOnFriday = GetWeekendsStartingOnFriday(DateTime.Today);

                int currentMonth = currentDate.Month;

                var monthTask = (from task in _dbContext.Task
                                     join employetask in _dbContext.EmployeeTask on task.Id equals employetask.TaskId
                                     join user in _dbContext.Users on employetask.EmployeeId equals user.Id
                                     where task.ProjectId == projectId && task.WeekEndingDate.Month == currentMonth     
                                     select new MonthlyTaskList()
                                     {
                                         Id = task.Id,
                                         EmoloyeeId= user.Id,
                                         EmployeeName= user.Name,
                                         ProjectId= task.ProjectId,
                                         Names = task.Name,
                                         Description = task.Description,
                                         Status = employetask.Status,
                                         percentage = task.Percentage,
                                         WeekEndingDate = task.WeekEndingDate,
                                         actualTime = employetask.ActTime,
                                         EstimedTime = employetask.EstTime,
                                         EstimatedStartedDate = task.EstimateStartDate,
                                         EstimatedEndedDate = task.EstimateEndDate,
                                     }).ToList();


                var ProjectTask = new ProjectTask()
                {
                    TotalTask = totalTask.Count(),
                    ProjectTaskLists = OverTaskList,
                    OnGoing = OnGoing,
                    Completed = Completed,
                    Assigned = Assigned,
                    ReadyForUAT = ReadyForUAT,
                    Unassigned = Unassigned,
                    WeekTask = createWeek,
                    monthlyTaskLists = monthTask
                };
                return ProjectTask; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ProjectCheckList>> GetUserTaskCheckList(int projectId)
        {
            try
            {
                var userTaskCheckList = _dbContext.UserTaskCheckList.Where(x => x.IsLatest == true && x.ProjectId== projectId).ToList();
                var taskList = await _dbContext.Task.ToListAsync();
                var users = await _dbContext.Users.ToListAsync();
                List<ProjectCheckList> projectCheckLists = new List<ProjectCheckList>();

                if (userTaskCheckList.Count > 0)
                { 

                    foreach (var obj in taskList)
                    {
                        var checkList = userTaskCheckList.Where(x => x.TaskId == obj.Id).ToList();

                        if (checkList.Count > 0)
                        {
                            ProjectCheckList projectCheckList = new ProjectCheckList()
                            {
                                TaskName = obj.Name,
                                QACheckCount = checkList.Where(x => x.IsQAChecked == true).Count(),
                                DevCheckCount = checkList.Where(x => x.IsDevChecked == true).Count(),
                                userTaskCheckLists = checkList
                            };
                            projectCheckLists.Add(projectCheckList);
                        }
                    }

                    if(projectCheckLists.Count > 0)
                    {

                    foreach(var obj in projectCheckLists)
                        {
                            foreach(var list in obj.userTaskCheckLists)
                            {
                                var username = users.FirstOrDefault(x => x.Id.ToString() == list.CreatedBy);
                                if (username != null)
                                {
                                    list.UserName = username.Name;
                                }
                            }

                        }
                    }

                }

                return projectCheckLists;
            }
            catch (Exception ex)
            {
                throw;
            }
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
        public static List<DateTime> GetWeekendsStartingOnFriday(DateTime currentDate)
        {
            List<DateTime> weekends = new List<DateTime>();
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            DateTime currentDay = firstDayOfMonth;

            // Loop through the days in the current month
            while (currentDay <= lastDayOfMonth)
            {
                // Check if the current day is Friday and the next day is Saturday
                if (currentDay.DayOfWeek == DayOfWeek.Friday && currentDay.AddDays(1).DayOfWeek == DayOfWeek.Saturday)
                {
                    weekends.Add(currentDay);
                }

                currentDay = currentDay.AddDays(1);
            }
            return weekends;
        }
    }
}
