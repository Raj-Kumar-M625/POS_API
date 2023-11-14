using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Constants;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services
{
    public class EmployeeTaskService : IEmployeeTaskService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;


        public EmployeeTaskService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<List<TaskDTO>> GetProjectTasklist(int Id)
        {
            try
            {
                var taskList = await (
                    from task in _dbContext.Task
                    where task.ProjectId == Id
                    select task
                ).ToListAsync();

                var empTaskList = await (
                    from empTask in _dbContext.EmployeeTask
                    where taskList.Select(t => t.Id).Contains(empTask.TaskId)
                    select empTask
                ).ToListAsync();

                var taskDTOList = taskList.Select(task => new TaskDTO
                {
                    Id = task.Id,
                    EmployeeTaskId = empTaskList.FirstOrDefault(et => et.TaskId == task.Id)?.Id ?? 0,
                    WeekEndingDate = empTaskList.FirstOrDefault(et => et.TaskId == task.Id)?.WeekEndingDate ?? task.WeekEndingDate,
                    ProjectId = task.ProjectId,
                    Name = task.Name,
                    Description = task.Description,
                    ActTime = task.ActTime,
                    EstTime = task.EstTime,
                    Status = empTaskList.FirstOrDefault(et => et.TaskId == task.Id)?.Status ?? task.Status,
                    Priority = task.Priority,
                    Percentage = task.Percentage,
                    CreatedDate = task.CreatedDate
                }).OrderByDescending(dto => dto.CreatedDate).ToList();

                return taskDTOList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Task> GetProjectTaskById(int Id)
        {
            try
            {
                var projectList = await _repository.Task.FindById(x => x.Id == Id);
                return projectList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EmployeeTask> AssignEmployeeTask(User user, EmployeeTaskDto dayPlan)
        {
            try
            {
                DateTime currentDate = DateTime.Today;
                /*  DateTime weekEndingDate;
                  int daysUntilFriday = (int)DayOfWeek.Friday - (int)currentDate.DayOfWeek;
                  if (daysUntilFriday <= 0)
                      daysUntilFriday += 7;
                  weekEndingDate = currentDate.AddDays(daysUntilFriday);
                  weekEndingDate = weekEndingDate.Date.AddDays(1).AddTicks(-1);*/
                var EmployeeDayPlan = _mapper.Map<EmployeeTask>(dayPlan);
                var day = await _repository.day.FindById(x => x.Date.Date == currentDate.Date);

                EmployeeDayPlan.Status = Status.InProgress;
                EmployeeDayPlan.CreatedBy = user.Id.ToString();
                EmployeeDayPlan.UpdatedBy = user.Id.ToString();
                EmployeeDayPlan.StartDate = DateTime.Now;
                EmployeeDayPlan.CreatedDate = DateTime.Now;
                EmployeeDayPlan.WeekEndingDate = dayPlan.WeekEndingDate;
                EmployeeDayPlan.DayId = day.Id;

                var taskcreated = await _repository.EmployeeTask.CreateAsync(EmployeeDayPlan);
                var taskList = await _repository.Task.FindByConditionAsync(x => x.Id == taskcreated.TaskId);
                var taskListDetails = taskList.First();
                taskListDetails.Status = Status.Assigned;
                var Updatetask = await _repository.Task.UpdateAsync(taskListDetails);
                Comments comment = new()
                {
                    ProjectId = dayPlan.ProjectId,
                    TaskId = dayPlan.TaskId,
                    EmployeeId = dayPlan.EmployeeId,
                    EmployeeTaskId = taskcreated.Id,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                    Comment = dayPlan.Comment,
                };
                var addComments = await _repository.Comments.CreateAsync(comment);


                return taskcreated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<bool> GetEmployeeTaskbyId(int EmployeeId, int TaskId)
        {
            try
            {
                var projectList = await _repository.EmployeeTask.FindByConditionAsync(x => x.EmployeeId == EmployeeId && x.TaskId == TaskId);
                var result = projectList.Any();

                return result;
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
                                   where comment.EmployeeTaskId != null
                                   select new CommentsDto()
                                   {
                                       Id = comment.Id,
                                       Comment = comment.Comment,
                                       Project = project.Name,
                                       Employee = user.Name,
                                       EmployeeTaskId = comment.EmployeeTaskId
                                   }).ToList();
                return commentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Task> GetTaskDetalisById(int TaskId)
        {
            try
            {
                var TaskDetalisList = await _repository.Task.FindById(x => x.Id == TaskId);
                return TaskDetalisList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<EmployeeTask> GetAssignedEmployeeTaskById(int TaskId, int projectId)
        {
            try
            {
                var TaskDetalisList = await _repository.EmployeeTask.FindById(x => x.TaskId == TaskId && x.ProjectId == projectId);
                return TaskDetalisList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*public async Task<List<EmployeeTaskDto>> GetWhatsapptaskListByTaskId( int employeeId, DateTime WeekEndingDate)
        {
            try
            {
                var taskDetailsList = await _repository.DailyTask.FindByConditionAsync(x => x.EmployeeId == employeeId && x.WeekEndingDate == WeekEndingDate);
                var empTaskId = taskDetailsList.Select(x => x.EmployeeTaskId).ToList();
                var emptaskDetails = await _repository.EmployeeTask.FindByConditionAsync(t => empTaskId.Contains(t.Id));
                //var taskIdList = taskDetailsList.Select(x => x.TaskId).ToList();
                //var taskDetails = await _repository.Task.FindByConditionAsync(t => taskIdList.Contains(t.Id));

                var taskDetailsWithNamesAndDescriptions = taskDetailsList
                    .Join(emptaskDetails,
                        empTask => empTask.EmployeeTaskId,
                        task => task.Id,
                        (empTask, task) => new EmployeeTaskDto
                        {
                            Id = empTask.Id,
                            Name = empTask.Name,
                            TaskDescription = empTask.Description,
                            Status = empTask.Status,
                            Percentage = empTask.Percentage,
                            ActTime = empTask.ActTime,
                            EstTime = empTask.EstTime,
                            ProjectId = task.ProjectId,
                            WorkedOn = empTask.WorkedOn,
                            EmployeeTaskId = empTask.EmployeeTaskId,
                            TaskId = task.TaskId
                        })
                    .ToList();

                var projectIds = taskDetailsWithNamesAndDescriptions.Select(x => x.ProjectId).ToList();
                var projects = await _repository.Project.FindByConditionAsync(p => projectIds.Contains(p.Id));

                foreach (var taskDetail in taskDetailsWithNamesAndDescriptions)
                {
                    var project = projects.FirstOrDefault(p => p.Id == taskDetail.ProjectId);
                    if (project != null)
                    {
                        taskDetail.ProjectName  = project.Name ;
                    }
                }

                return taskDetailsWithNamesAndDescriptions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
*/
        public async Task<List<EmployeeTaskDto>> GetWhatsapptaskListByTaskId(int employeeId, DateTime weekEndingDate)
        {
            try
            {
                IEnumerable<EmployeeDailyTask> taskDetailsList;
                DateTime startDate;
                var indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var indianTimeNow = TimeZoneInfo.ConvertTime(DateTime.Now, indianTimeZone);

                if (indianTimeNow.DayOfWeek == DayOfWeek.Saturday)
                {
                    startDate = weekEndingDate.AddDays(1).Date;
                    taskDetailsList = await _repository.DailyTask.FindByConditionAsync(x => x.EmployeeId == employeeId && x.WorkedOn == startDate);
                }
                else
                {
                   taskDetailsList = await _repository.DailyTask.FindByConditionAsync(x => x.EmployeeId == employeeId && x.WeekEndingDate == weekEndingDate);
                }


                var empTaskId = taskDetailsList.Select(x => x.EmployeeTaskId).ToList();
                var emptaskDetails = await _repository.EmployeeTask.FindByConditionAsync(t => empTaskId.Contains(t.Id));

                var taskDetailsWithNamesAndDescriptions = taskDetailsList
                    .Join(emptaskDetails,
                        empTask => empTask.EmployeeTaskId,
                        task => task.Id,
                        (empTask, task) => new EmployeeTaskDto
                        {
                            Id = empTask.Id,
                            Name = empTask.Name,
                            TaskDescription = empTask.Description,
                            Status = empTask.Status,
                            Percentage = empTask.Percentage,
                            ActTime = empTask.ActTime,
                            EstTime = empTask.EstTime,
                            ProjectId = task.ProjectId,
                            WorkedOn = empTask.WorkedOn,
                            EmployeeTaskId = empTask.EmployeeTaskId,
                            TaskId = task.TaskId
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



        public async Task<EmployeeTask> MoveTask(User user, EmployeeTaskDto task)
        {
            try
            {
                var taskFromdb = _dbContext.EmployeeTask.FirstOrDefault(x => x.Id == task.Id);
                var updateTask = _dbContext.Task.FirstOrDefault(x => x.Id == taskFromdb.TaskId);

                if (updateTask != null)
                {
                    updateTask.EstTime = task.EstTime + updateTask.EstTime - taskFromdb.ActTime;
                    _dbContext.Task.Update(updateTask);
                    _dbContext.SaveChanges();
                }

                if (taskFromdb != null)
                {
                    taskFromdb.Status = "Moved";
                    taskFromdb.UpdatedDate = DateTime.Now;
                    taskFromdb.UpdatedBy = user.Id.ToString();
                    _dbContext.EmployeeTask.Update(taskFromdb);
                    _dbContext.SaveChanges();
                }
                DateTime currentDate = DateTime.Today;
                var day = await _repository.day.FindById(x => x.Date.Date == currentDate.Date);
                var EmployeeTask = new EmployeeTask();
                EmployeeTask.EmployeeId = taskFromdb.EmployeeId;
                EmployeeTask.TaskId = taskFromdb.TaskId;
                EmployeeTask.ProjectId = taskFromdb.ProjectId;
                EmployeeTask.StartDate = (DateTime)task.StartDate;
                EmployeeTask.EndDate = (DateTime)task.EndDate;
                EmployeeTask.EstTime = task.EstTime;
                EmployeeTask.ActTime = taskFromdb.ActTime;
                EmployeeTask.WeekEndingDate = task.WeekEndingDate;
                EmployeeTask.Status = "Assigned";
                EmployeeTask.Priority = taskFromdb.Priority;
                EmployeeTask.Percentage = taskFromdb.Percentage;
                EmployeeTask.CreatedDate = DateTime.Now;
                EmployeeTask.CreatedBy = user.Id.ToString();
                EmployeeTask.UpdatedDate = DateTime.Now;
                EmployeeTask.UpdatedBy = user.Id.ToString();
                EmployeeTask.EstEndDate = taskFromdb.EstEndDate;
                EmployeeTask.EstStartDate = taskFromdb.EstStartDate;
                EmployeeTask.ProjectObjectiveId = taskFromdb.ProjectObjectiveId;
                EmployeeTask.DayId = day.Id;

                var employeetaskcreated = await _repository.EmployeeTask.CreateAsync(EmployeeTask);

                Comments comment = new()
                {
                    ProjectId = taskFromdb.ProjectId,
                    TaskId = taskFromdb.TaskId,
                    EmployeeId = taskFromdb.EmployeeId,
                    EmployeeTaskId = employeetaskcreated.Id,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                    Comment = task.Comment,
                };
                var addComments = await _repository.Comments.CreateAsync(comment);
                return employeetaskcreated;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> ReassignTask(User user, EmployeeTaskDto employeeTaskDto)
        {
            try
            {
                var employeeTask = _dbContext.EmployeeTask.FirstOrDefault(x => x.Id == employeeTaskDto.Id);

                var day = await _repository.day.FindById(x => x.Date.Date == DateTime.Now.Date);

                var reAssignedTask = new EmployeeTask();
                reAssignedTask.TaskId = employeeTask.TaskId;
                reAssignedTask.ProjectId = employeeTask.ProjectId;
                reAssignedTask.StartDate = employeeTask.StartDate;
                reAssignedTask.EndDate = employeeTask.EndDate;
                reAssignedTask.ActTime = employeeTask.ActTime;
                reAssignedTask.WeekEndingDate = employeeTask.WeekEndingDate;
                reAssignedTask.Status = "Assigned";
                reAssignedTask.Priority = employeeTask.Priority;
                reAssignedTask.Percentage = employeeTask.Percentage;
                reAssignedTask.ProjectObjectiveId = employeeTask.ProjectObjectiveId;
                reAssignedTask.EmployeeId = (int)employeeTaskDto.EmployeeId;
                reAssignedTask.EstStartDate = employeeTaskDto.EstStartDate;
                reAssignedTask.EstEndDate = employeeTaskDto.EstEndDate;
                reAssignedTask.DayId = day.Id;
                reAssignedTask.CreatedDate = DateTime.Now;
                reAssignedTask.CreatedBy = user.Id.ToString();
                reAssignedTask.UpdatedDate = DateTime.Now;
                reAssignedTask.UpdatedBy = user.Id.ToString();
                reAssignedTask.EstTime = employeeTaskDto.EstTime;
                _dbContext.EmployeeTask.Add(reAssignedTask);

                employeeTask.Status = "Reassigned";
                employeeTask.UpdatedDate = DateTime.Now;
                employeeTask.UpdatedBy = user.Id.ToString();
                _dbContext.EmployeeTask.Update(employeeTask);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
