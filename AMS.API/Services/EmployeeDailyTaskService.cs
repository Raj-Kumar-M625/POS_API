using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProjectOversight.API.Constants;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services
{
    public class EmployeeDailyTaskService : IEmployeeDailyTaskService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public EmployeeDailyTaskService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<List<EmployeeDailyTaskDto>> GetTimePlanList(User sessionUser)
        {
            try
            {
                Random rnd = new Random();
                DateTime dateTime = DateTime.Now.AddDays(-60);
                var timePlanList = (from employeeDailyTask in _dbContext.EmployeeDailyTask
                                    join employee in _dbContext.Employee on employeeDailyTask.EmployeeId equals employee.Id into employeeGroup
                                    from employee in employeeGroup.DefaultIfEmpty()
                                        //join user in _dbContext.Users on employee.UserId equals user.Id into userGroup
                                        //from user in userGroup.DefaultIfEmpty()
                                        //join comment in _dbContext.Comments on employeeDailyTask.Id equals comment.EmployeeDailyTaskId into commentGroup
                                        //from comment in commentGroup.DefaultIfEmpty()
                                    join project in _dbContext.Project on employeeDailyTask.ProjectId equals project.Id into projectGroup
                                    from project in projectGroup.DefaultIfEmpty()
                                        //join employeeTask in _dbContext.EmployeeTask on employeeDailyTask.EmployeeTaskId equals employeeTask.Id into employeeTaskGroup
                                        //from employeeTask in employeeTaskGroup.DefaultIfEmpty()
                                        //join task in _dbContext.Task on employeeTask.TaskId equals task.Id into taskGroup
                                        //from task in taskGroup.DefaultIfEmpty()
                                    where employeeDailyTask.CreatedDate.HasValue && employeeDailyTask.CreatedDate.Value.Date > dateTime.Date
                                    select new EmployeeDailyTaskDto()
                                    {
                                        Id = rnd.Next(5000000),
                                        Name = employeeDailyTask.Name,
                                        Description = employeeDailyTask.Description,
                                        EmployeeName = employee.Name,
                                        ProjectName = project.Name,
                                        StartDate = employeeDailyTask.StartDate,
                                        EndDate = employeeDailyTask.EndDate,
                                        EstTime = employeeDailyTask.EstTime,
                                        ActTime = employeeDailyTask.ActTime,
                                        WeekEndingDate = employeeDailyTask.WeekEndingDate,
                                        Status = employeeDailyTask.Status,
                                        Priority = employeeDailyTask.Priority,
                                        Percentage = employeeDailyTask.Percentage,
                                        ProjectId = project.Id,
                                    }).OrderByDescending(x => x.StartDate).ToList();
                if (sessionUser.UserType == "Customer")
                {
                    var employee = await _dbContext.Employee.FirstOrDefaultAsync(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == employee.Id).ToListAsync();
                    timePlanList = timePlanList.Where(x => customerProject.Any(cp => cp.ProjectId == x.ProjectId)).ToList();

                }

                return timePlanList;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<EmployeeDailyTaskDto>> GetEmployeeTask(int EmployeeId)
        {
            try
            {
                var employeeTaskList = _dbContext.EmployeeDailyTask.Where(x => x.EmployeeId == EmployeeId).Include(x => x.EmployeeTask).OrderByDescending(x => x.CreatedDate).ToList();
                var employeeDaily = _mapper.Map<IEnumerable<EmployeeDailyTaskDto>>(employeeTaskList);
                return employeeDaily;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EmployeeDailyTask> AddEmployeeDailyTask(User user, EmployeeDailyTaskDto employeeDailyTask)
        {
            try
            {
                DateTime currentDate = DateTime.Today;
                DateTime weekEndingDate;
                int daysUntilFriday = (int)DayOfWeek.Friday - (int)currentDate.DayOfWeek;
                if (daysUntilFriday <= 0)
                    daysUntilFriday += 7;
                weekEndingDate = currentDate.AddDays(daysUntilFriday);
                weekEndingDate = weekEndingDate.Date.AddDays(1).AddTicks(-1);
                var employeeDaily = await _repository.DailyTask.FindById(x => x.Id == employeeDailyTask.Id);
                var task = await _repository.Task.FindById(x => x.Id == employeeDailyTask.TaskId);
                var category = await _repository.Category.FindById(x => x.Id == task.CategoryId);
                UserStoryUI userStoryUI = null;
                if (task.UserStoryUIId == 0)
                {
                    userStoryUI = null;
                }
                else
                {
                    userStoryUI = await _repository.UserStoryUI.FindById(x => x.Id == task.UserStoryUIId);
                }
                var projectUS = await _repository.UserStory.FindByConditionAsync(x => x.ProjectId == task.ProjectId);
                var project = await _repository.Project.FindById(x => x.Id == task.ProjectId);
                UserInterface ui = new();
                IEnumerable<UserStory> us = new List<UserStory>();
                if (userStoryUI != null)
                {
                    ui = await _repository.UserInterface.FindById(x => x.Id == userStoryUI.UIId);
                    us = await _repository.UserStory.FindByConditionAsync(x => x.Id == userStoryUI.UserStoryId);
                }
                var employeeTask = await _repository.EmployeeTask.FindById(x => x.Id == employeeDailyTask.EmployeeTaskId);
                {
                    employeeDaily.StartDate = employeeDailyTask.StartDate ?? DateTime.Today;
                    employeeDaily.EndDate = employeeDailyTask.EndDate ?? DateTime.Today;
                    employeeDaily.ActTime = employeeDailyTask.ActTime ?? 0;
                    employeeDaily.Percentage = employeeDailyTask.Percentage ?? 0;
                    employeeDaily.CreatedDate = DateTime.Now;
                    employeeDaily.CreatedBy = user.Id.ToString();
                    employeeDaily.UpdatedDate = DateTime.Now;
                    employeeDaily.UpdatedDate = DateTime.Now;
                    employeeDaily.UpdatedBy = user.Id.ToString();
                    if (employeeDaily.Percentage == 100 && category.Categories == "Development" && task.Category.SubCategory == "UILevelTask")
                    {
                        employeeDaily.Status = "Ready-For-UAT";
                    }
                    else if (employeeDaily.Percentage == 100)
                    {
                        employeeDaily.Status = "Completed";
                    }
                    else
                    {
                        employeeDaily.Status = "In-Progress";
                    }
                }
                var dailyTaskcreated = await _repository.DailyTask.UpdateAsync(employeeDaily);
                {
                    employeeTask.Percentage = employeeDailyTask.Percentage ?? 0;
                    employeeTask.StartDate = employeeDailyTask.StartDate ?? DateTime.Today;
                    employeeTask.EndDate = employeeDailyTask.EndDate ?? DateTime.Today;
                    employeeTask.ActTime = employeeDailyTask.ActTime ?? 0;
                    employeeTask.UpdatedDate = DateTime.Now;
                    employeeTask.UpdatedBy = user.Id.ToString();
                    if (employeeTask.Percentage == 100 && category.Categories == "Development" && category.SubCategory == "UILevelTask")
                    {
                        employeeTask.Status = "Ready-For-UAT";
                    }
                    else if (employeeTask.Percentage == 100)
                    {
                        employeeTask.Status = "Completed";
                    }
                    else
                    {
                        employeeTask.Status = "In-Progress";
                    }
                }
                var updateEmployeeTask = await _repository.EmployeeTask.UpdateAsync(employeeTask);
                {
                    task.Percentage = employeeDailyTask.Percentage ?? 0;
                    task.ActualStartDate = employeeDailyTask.StartDate;
                    task.ActualEndDate = employeeDailyTask.EndDate;
                    task.ActTime = employeeDailyTask.ActTime ?? 0;
                    task.UpdatedDate = DateTime.Now;
                    task.UpdatedBy = user.Id.ToString();
                    if (task.Percentage == 100 && category.Categories == "Development" && category.SubCategory == "UILevelTask")
                    {
                        task.Status = "Ready-For-UAT";
                    }
                    else if (task.Percentage == 100)
                    {
                        task.Status = "Completed";
                    }
                    else
                    {
                        task.Status = "In-Progress";
                    }
                }
                var updateTask = await _repository.Task.UpdateAsync(task);
                {
                    ui.Percentage = employeeDailyTask.Percentage ?? 0;
                    ui.UpdatedDate = DateTime.Now;
                    ui.UpdatedBy = user.Id.ToString();
                }
                if (ui.Id != 0)
                {
                    var updateUI = await _repository.UserInterface.UpdateAsync(ui);
                }
                if (us.Count() != 0)
                {
                    int computedPercentage;
                    computedPercentage = employeeDailyTask.Percentage?? 0 / us.Count();
                    foreach (UserStory story in us)
                    {
                        story.Percentage = computedPercentage;
                        story.UpdatedDate = DateTime.Now;
                        story.UpdatedBy = user.Id.ToString();
                        await _repository.UserStory.UpdateAsync(story);
                    }

                }
                Comments comment = new()
                {
                    ProjectId = employeeTask.ProjectId,
                    TaskId = employeeTask.TaskId,
                    EmployeeId = employeeTask.EmployeeId,
                    EmployeeTaskId = employeeTask.Id,
                    EmployeeDailyTaskId = dailyTaskcreated.Id,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                    Comment = employeeDailyTask.Comment,
                };
                var addComments = await _repository.Comments.CreateAsync(comment);

                foreach (var taskchecklistid in employeeDailyTask.TaskChecklistId)
                {
                    var taskchecklist = await _repository.TaskCheckList.FindByConditionAsync(x => x.Id == taskchecklistid);
                    if (category.Categories == "Development" && category.SubCategory == "UILevelTask")
                    {
                        var UserTaskCheckListsDevCheck = await _repository.UserTaskCheckList.FindByConditionAsync(x => x.TaskCheckListId == taskchecklist.First().Id && x.IsDevChecked == true);

                        foreach (var existingRecord in UserTaskCheckListsDevCheck)
                        {
                            existingRecord.IsLatest = false;
                            await _repository.UserTaskCheckList.UpdateAsync(existingRecord);
                        }
                    }
                    else if (category.Categories == "QA" && category.SubCategory == "Testing")
                    {
                        var UserTaskCheckListsQAChecked = await _repository.UserTaskCheckList.FindByConditionAsync(x => x.TaskCheckListId == taskchecklist.First().Id && x.IsQAChecked == true);
                        foreach (var existingRecord in UserTaskCheckListsQAChecked)
                        {
                            existingRecord.IsLatest = false;
                            await _repository.UserTaskCheckList.UpdateAsync(existingRecord);
                        }
                    }

                    // Create a new UserTaskCheckList
                    var userTaskCheckList = new UserTaskCheckList
                    {
                        TaskId = task.Id,
                        TaskCheckListId = taskchecklistid,
                        ProjectId = task.ProjectId,
                        USId = taskchecklist.First().USId,
                        UIId = taskchecklist.First().UIId,
                        CategoryId = taskchecklist.First().CategoryId,
                        CheckListDescription = taskchecklist.First().CheckListDescription,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString(),
                        UpdatedDate = DateTime.Now,
                        UserStoryUIId = taskchecklist.First().UserStoryUIId
                    };

                    if (category.Categories == "Development" && category.SubCategory == "UILevelTask")
                    {
                        userTaskCheckList.IsDevChecked = true;
                    }
                    else if (category.Categories == "QA" && category.SubCategory == "Testing")
                    {
                        userTaskCheckList.IsQAChecked = true;
                    }

                    userTaskCheckList.IsLatest = true;
                    var addUserCheckList = await _repository.UserTaskCheckList.CreateAsync(userTaskCheckList);
                }

                return employeeDaily;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<EmployeeDailyTaskDto>> GetEmployeeTimePlanList(int employeeId)
        {
            try
            {
                var timePlanList = (from employeeDailyTask in _dbContext.EmployeeDailyTask
                                    join employee in _dbContext.Employee on employeeDailyTask.EmployeeId equals employee.Id
                                    join user in _dbContext.Users on employee.UserId equals user.Id
                                    join project in _dbContext.Project on employeeDailyTask.ProjectId equals project.Id into projectGroup
                                    from project in projectGroup.DefaultIfEmpty()
                                    //join employeeTask in _dbContext.EmployeeTask on employeeDailyTask.EmployeeTaskId equals employeeTask.Id into employeeTaskGroup
                                    //from employeeTask in employeeTaskGroup.DefaultIfEmpty()
                                    //join task in _dbContext.Task on employeeTask.TaskId equals task.Id into taskGroup
                                    //from task in taskGroup.DefaultIfEmpty()
                                    where employeeDailyTask.EmployeeId == employeeId
                                    select new EmployeeDailyTaskDto()
                                    {
                                        Id = employeeDailyTask.Id,
                                        Name = employeeDailyTask.Name,
                                        Description = employeeDailyTask.Description,
                                        EmployeeName = user.Name,
                                        ProjectName = project.Name,
                                        StartDate = employeeDailyTask.StartDate,
                                        EndDate = employeeDailyTask.EndDate,
                                        EstTime = employeeDailyTask.EstTime,
                                        ActTime = employeeDailyTask.ActTime,
                                        WeekEndingDate = employeeDailyTask.WeekEndingDate,
                                        Status = employeeDailyTask.Status,
                                        Priority = employeeDailyTask.Priority,
                                        Percentage = employeeDailyTask.Percentage
                                    }).ToList();
                return timePlanList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CommentsDto>> GetComments()
        {
            try
            {
                var commentList = (from comment in _dbContext.Comments
                                   join project in _dbContext.Project on comment.ProjectId equals project.Id
                                   join employee in _dbContext.Employee on comment.EmployeeId equals employee.Id
                                   join user in _dbContext.Users on employee.UserId equals user.Id
                                   where comment.EmployeeDailyTaskId != null
                                   select new CommentsDto()
                                   {
                                       Id = comment.Id,
                                       Comment = comment.Comment,
                                       Project = project.Name,
                                       Employee = user.Name,
                                       EmployeeDailyTaskId = comment.EmployeeDailyTaskId
                                   }).ToList();
                return commentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EmployeeDailyTask> GetEmployeeDailyTaskById(int employeeId, int projectId)
        {
            try
            {
                var TaskDetalisList = await _repository.DailyTask.FindByConditionAsync(x => x.EmployeeId == employeeId && x.ProjectId == projectId);
                return TaskDetalisList.LastOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<EmployeeDailyTask>> GetEmployeeDailyTask(int employeeTaskId)
        {
            var employeeDailyTasks = (from employeeDailyTask in _dbContext.EmployeeDailyTask
                                      join user in _dbContext.Users on employeeDailyTask.CreatedBy equals user.Id.ToString()
                                      where employeeDailyTask.EmployeeTaskId == employeeTaskId
                                      select new EmployeeDailyTask()
                                      {
                                          Name = employeeDailyTask.Name,
                                          Description = employeeDailyTask.Description,
                                          EmployeeTaskId = employeeDailyTask.EmployeeTaskId,
                                          CreatedBy = user.Name,
                                          Percentage = employeeDailyTask.Percentage,
                                          StartDate = employeeDailyTask.StartDate,
                                          EndDate = employeeDailyTask.EndDate,
                                          Status = employeeDailyTask.Status,
                                          ActTime = employeeDailyTask.ActTime,
                                          EstTime = employeeDailyTask.EstTime
                                      }).OrderBy(x => x.Percentage).ToList();
            return employeeDailyTasks;
        }

        public async Task<List<EmployeeTaskDto>> GetCompletedWhatsapptaskListByTaskId(int employeeId, DateTime WeekEndingDate)
        {
            try
            {
                var taskDetailsList = await _repository.EmployeeTask.FindByConditionAsync(x => x.EmployeeId == employeeId && x.WeekEndingDate == WeekEndingDate);
                var taskIdList = taskDetailsList.Select(x => x.TaskId).ToList();
                var taskDetails = await _repository.Task.FindByConditionAsync(t => taskIdList.Contains(t.Id));

                var taskDetailsWithNamesAndDescriptions = taskDetailsList
                    .Join(taskDetails,
                        empTask => empTask.TaskId,
                        task => task.Id,
                        (empTask, task) => new EmployeeTaskDto
                        {
                            Id = empTask.Id,
                            Name = task.Name,
                            TaskDescription = task.TaskDescription,
                            EstTime = empTask.EstTime,
                            ProjectId = task.ProjectId,
                            Status = empTask.Status,
                        })
                    .ToList();

                var projectIds = taskDetailsWithNamesAndDescriptions.Select(x => x.ProjectId).ToList();
                var projects = await _repository.Project.FindByConditionAsync(p => projectIds.Contains(p.Id));

                foreach (var taskDetail in taskDetailsWithNamesAndDescriptions)
                {
                    var project = projects.FirstOrDefault(p => p.Id == taskDetail.ProjectId);
                    if (project != null)
                    {
                        taskDetail.ProjectName = project.Name;
                    }
                }

                return taskDetailsWithNamesAndDescriptions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<EachEmployeeTask> GetEachEmployeeDailyTaskById(int employeeId)
        {
            try
            {
                var employeeProjects = (from employeeProject in _dbContext.EmployeeProject
                                        join project in _dbContext.Project on employeeProject.ProjectId equals project.Id
                                        where employeeProject.EmployeeId == employeeId
                                        select new Project()
                                        {
                                            Id = project.Id,
                                            Name = project.Name,
                                            Type = project.Type,
                                            Status = project.Status,
                                            Percentage = project.Percentage,
                                        }).ToList();
                var employeeTasks =  _dbContext.EmployeeTask.Where(x => x.EmployeeId == employeeId).ToList();
              
                int totalCompleted = employeeTasks.Where (x => x.Status == "Completed").Count();
                int totalInProgress = employeeTasks.Where(x => x.Status == "In-Progress").Count();
                int totalReadyForUAT = employeeTasks.Where(x => x.Status == "Ready-For-UAT").Count();
                int IdOccurrences = employeeProjects.Count;
                int EmployeeTaskIdOccurrences = employeeTasks.Count;
                Decimal totalEstTime = employeeTasks.Sum(x => x.EstTime);
                Decimal totalActTime = employeeTasks.Sum(x => x.ActTime);

                List<DateTime> weekendsStartingOnFriday = GetWeekendsStartingOnFriday(DateTime.Today);


                var weeklyLists = new List<WeeklyListDto>();
                // Print the list of weekends starting on Friday
                foreach (var weekend in weekendsStartingOnFriday)
                {
                    var weekly = new WeeklyListDto();
                    // Get EmployeeTasks that have a WeekEndingDate matching the current weekend
                    var empTasksForWeekend = _dbContext.EmployeeTask
                        .Where(x => x.WeekEndingDate.Date == weekend.Date && x.EmployeeId == employeeId);

                    weekly.WeekEndDate = weekend.Date;
                    weekly.EstTime = empTasksForWeekend.Sum(x => x.EstTime);
                    weekly.ActTime = empTasksForWeekend.Sum(x => x.ActTime);
                    weeklyLists.Add(weekly);
                }

                EachEmployeeTask eachEmployeeTask = new EachEmployeeTask()
                {

                    EmployeeProjects = employeeProjects,
                    ToatalEstTime = totalEstTime,
                    ToatalActTime = totalActTime,
                    TotalProject = IdOccurrences,
                    TotalTask = EmployeeTaskIdOccurrences,
                    TotalCompleted = totalCompleted,
                    TotalReadyForUAT = totalReadyForUAT,
                    TotalInProgress = totalInProgress,
                    WeeklyLists = weeklyLists,
                };

                return eachEmployeeTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<EachEmployeeTask> GetEmployeeMonthlyWise(int employeeId)
        {
            try
            {
                int year = DateTime.Now.Year; // Replace with the desired year


                var MonthlyLists = new List<MonthlyLists>();
                var list = new List<MonthlyLists>();

                for (int i = 1; i <= 12; i++)
                {
                    var Acttime = _dbContext.EmployeeTask.Where(x => x.StartDate.Month == i && x.EmployeeId == employeeId && x.StartDate.Year == year).Sum(x => x.ActTime);
                    var EstTime = _dbContext.EmployeeTask.Where(x => x.StartDate.Month == i && x.EmployeeId == employeeId && x.StartDate.Year == year).Sum(x => x.EstTime);
                    int totalCompleted = _dbContext.EmployeeTask.Where(x => x.Status == "Completed" && x.CreatedDate.HasValue && x.CreatedDate.Value.Month == i && x.EmployeeId == employeeId && x.CreatedDate.HasValue && x.CreatedDate.Value.Year == year).Count();
                    int totalInProgress = _dbContext.EmployeeTask.Where(x => x.Status == "In-Progress" && x.CreatedDate.HasValue && x.CreatedDate.Value.Month == i && x.EmployeeId == employeeId && x.CreatedDate.HasValue && x.CreatedDate.Value.Year == year).Count();
                    int totalReadyForUAT = _dbContext.EmployeeTask.Where(x => x.Status == "Ready-For-UAT" && x.CreatedDate.HasValue && x.CreatedDate.Value.Month == i && x.EmployeeId == employeeId && x.CreatedDate.HasValue && x.CreatedDate.Value.Year == year).Count();
                    MonthlyLists monthlyHours = new MonthlyLists();
                    monthlyHours.month = i;
                    monthlyHours.ActTime = Acttime;
                    monthlyHours.EstTime = EstTime;
                    monthlyHours.totalCompleted = totalCompleted;
                    monthlyHours.totalInProgress = totalInProgress;
                    monthlyHours.totalReadyForUAT = totalReadyForUAT;
                    MonthlyLists.Add(monthlyHours);
                }
                EachEmployeeTask eachEmployeeTask = new EachEmployeeTask()
                {
                    MonthlyLists = MonthlyLists,
                };

                return eachEmployeeTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public static Dictionary<int, List<DateTime>> GetMonthlyWeekendsStartingOnFriday(int year)
        {
            Dictionary<int, List<DateTime>> monthlyWeekends = new Dictionary<int, List<DateTime>>();

            for (int month = 1; month <= 12; month++)
            {
                DateTime firstDayOfMonth = new DateTime(year, month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                DateTime currentDay = firstDayOfMonth;

                List<DateTime> weekends = new List<DateTime>();

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

                if (weekends.Count > 0)
                {
                    monthlyWeekends.Add(month, weekends);
                }
            }

            return monthlyWeekends;
        }


        public async Task<EmployeeDailyTask> AddEmployeeDayPlan(User user, EmployeeDailyTaskDto employeeDailyTask)
        {
            try
            {
                DateTime currentDate = DateTime.Today;
                DateTime weekEndingDate;
                int daysUntilFriday = (int)DayOfWeek.Friday - (int)currentDate.DayOfWeek;
                if (daysUntilFriday < 0)
                    daysUntilFriday += 7;
                weekEndingDate = currentDate.AddDays(daysUntilFriday);
                //weekEndingDate = weekEndingDate.Date.AddDays(1).AddTicks(-1);
                var employeeDaily = _mapper.Map<EmployeeDailyTask>(employeeDailyTask);
                var task = await _repository.Task.FindById(x => x.Id == employeeDailyTask.TaskId);
                var category = await _repository.Category.FindById(x => x.Id == task.CategoryId);
                var employeeTask = await _repository.EmployeeTask.FindById(x => x.Id == employeeDailyTask.EmployeeTaskId);
                {
                    employeeDaily.WeekEndingDate = weekEndingDate;
                    employeeDaily.StartDate = DateTime.Now;
                    employeeDaily.EndDate = DateTime.Now;
                    employeeDaily.EstTime = employeeDailyTask.EstTime;
                    employeeDaily.Name = employeeDailyTask.Name;
                    employeeDaily.Percentage = employeeTask.Percentage;
                    employeeDaily.Description = employeeDailyTask.Description;
                    employeeDaily.CreatedDate = DateTime.Now;
                    employeeDaily.CreatedBy = user.Id.ToString();
                    employeeDaily.UpdatedDate = DateTime.Now;
                    employeeDaily.UpdatedDate = DateTime.Now;
                    employeeDaily.UpdatedBy = user.Id.ToString();
                    employeeDaily.WorkedOn = employeeDailyTask.WorkedOn;
                }
                var dailyTaskcreated = await _repository.DailyTask.CreateAsync(employeeDaily);
                var day = await _repository.day.FindById(x => x.Date == employeeDailyTask.WorkedOn);
                var existingWork = await _repository.WorkFlow.FindByConditionAsync(x =>
                         x.WorkFlowId == 2 && x.WorkedOnDate.Value.Date == employeeDailyTask.WorkedOn.Value.Date && x.EmployeeId == employeeDailyTask.EmployeeId);
                if (existingWork.Count() == 0)
                {
                    WorkFlow workFlow = new WorkFlow
                    {
                        EmployeeId = employeeDailyTask.EmployeeId,
                        DayId = day.Id,
                        WorkFlowId = 2,
                        WorkedOnDate = employeeDailyTask.WorkedOn,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString(),
                        UpdatedDate = DateTime.Now,
                    };
                    await _repository.WorkFlow.CreateAsync(workFlow);
                }
                //var existingWork = await _repository.EmployeeWorkFlow.FindByConditionAsync(x => x.EmployeeId == employeeDailyTask.EmployeeId
                //&& x.DayId == day.Id);
                //var isexistingWork = existingWork.Any(x => x.WorkFlowId == 3);
                //if(isexistingWork == false)
                //{
                //    EmployeeWorkFlow workFlow = new EmployeeWorkFlow
                //   {
                //       EmployeeId = employeeDailyTask.EmployeeId,
                //       DayId = day.Id,
                //      WorkFlowId = 3,
                //   };
                //   await _repository.EmployeeWorkFlow.CreateAsync(workFlow);
                //}
                return employeeDaily;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EmployeeDailyStatDto> GetEmployeeTaskStat(int employeeId, int employeeTaskId)
        {
            try
            {
                var taskDetailsList = await _repository.EmployeeTask.FindByConditionAsync(x => x.EmployeeId == employeeId && x.Id == employeeTaskId);
                var dailyTaskList = await _repository.DailyTask.FindByConditionAsync(x => x.EmployeeId == employeeId && x.EmployeeTaskId == employeeTaskId);

                EmployeeDailyStatDto employeeDailyStatDto = new EmployeeDailyStatDto()
                {
                    DailyTaskAct = dailyTaskList.Sum(x => x.ActTime),
                    DailyTaskEst = dailyTaskList.Sum(x => x.EstTime),
                    EmployeeTaskEst = taskDetailsList.Sum(x => x.EstTime)
                };
                return employeeDailyStatDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<Task>> GetEmployeeDevChecklist(int TaskId)
        {
            try
            {
                var taskDetailsList = await _repository.Task.FindByConditionAsync(x => x.Id == TaskId);
                var UserStoryUIId = taskDetailsList.Select(x => x.UserStoryUIId).ToList();
                var categorydetails = await _repository.Category.FindByConditionAsync(x=>x.Id == taskDetailsList.First().CategoryId);
                var TaskChecklist = await _repository.TaskCheckList.FindByConditionAsync(x => UserStoryUIId.Contains(x.UserStoryUIId));

                var EmployeeCategory = taskDetailsList
                    .Join(TaskChecklist,
                        task => task.UserStoryUIId,
                        Taskchecklist => Taskchecklist.UserStoryUIId,
                        (task, Taskchecklist) => new Task
                        {
                            Id = task.Id,
                            UserStoryUIId = Taskchecklist.UserStoryUIId,
                            Category = categorydetails.First()

                        })
                    .ToList();

                return EmployeeCategory;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TaskCheckList>> GetEmployeeTaskChecklist(int UserStoryUIID)
        {
            try
            {
                var taskCheckList = await _repository.TaskCheckList.FindByConditionAsync(x => x.UserStoryUIId == UserStoryUIID);

                if (taskCheckList == null || !taskCheckList.Any())
                {
                    return new List<TaskCheckList>();
                }
                foreach (var taskCheck in taskCheckList)
                {
                    var userTaskCheckList = await _repository.UserTaskCheckList.FindByConditionAsync(x => x.TaskCheckListId == taskCheck.Id && x.IsLatest == true);
                    taskCheck.userTaskCheckList = userTaskCheckList.ToList(); 
                }
                return taskCheckList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<EmployeeDailyTask>> AutoTaskChangeStatus()
        {
            try
            {
                var taskDetailsList = await _repository.DailyTask.FindByConditionAsync(x => x.ActTime == 0);
                foreach (var task in taskDetailsList)
                {
                    task.Status = "NOT-Updated";
                    task.UpdatedBy = "System";
                    task.UpdatedDate = DateTime.Now;
                    await _repository.DailyTask.UpdateAsync(task);
                }

                // Return the updated taskDetailsList
                return taskDetailsList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
