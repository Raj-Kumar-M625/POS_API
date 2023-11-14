using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Data;
using ProjectOversight.API.Services.Interface;
using ProjectOversight.API.Constants;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services
{
    public class ReleaseNotesService : IReleaseNotesService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public ReleaseNotesService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<List<Data.Model.Task>> GetAllReadyForUATTasklist(int projectId)
        {
            try
            {
                var projectList = await _repository.Task.FindByConditionAsync(x => x.ProjectId == projectId && (x.Status == Status.ReadyForUAT || x.Status == "Ready For UAT"));
                var result = projectList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Data.Model.Task>> UpdateInUATTask(List<int> projectId)
        {
            try
            {
                const string InUATStatus = "IN-UAT";

                // Update Task table
                var taskList = await _repository.Task.FindByConditionAsync(x => projectId.Contains(x.Id) && (x.Status == Status.ReadyForUAT || x.Status == "Ready For UAT"));
                var taskLists = taskList.Select(x => x.Id).ToList();
                foreach (var task in taskList)
                {
                    task.Status = InUATStatus;
                    await _repository.Task.UpdateAsync(task);
                }
                var employeeDailyList = await _repository.EmployeeTask.FindByConditionAsync(x => taskLists.Contains(x.TaskId));
                var employeeDailyLists = employeeDailyList.Select(x => x.Id).ToList();
                foreach (var employeeTask in employeeDailyList)
                {
                    employeeTask.Status = InUATStatus;
                    await _repository.EmployeeTask.UpdateAsync(employeeTask);
                }
                var employeeTaskList = await _repository.DailyTask.FindByConditionAsync(x => employeeDailyLists.Contains(x.EmployeeTaskId));
                foreach (var dailyTask in employeeTaskList)
                {
                    dailyTask.Status = InUATStatus;
                    await _repository.DailyTask.UpdateAsync(dailyTask);
                }
                return taskList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddReleaseNotes(User user, ReleaseNotes releaseNote)
        {
            try
            {

                ReleaseNotes releaseNotes = new ReleaseNotes()
                {
                    ProjectId = releaseNote.ProjectId,
                    Version = releaseNote.Version,
                    Description = releaseNote.Description,
                    ReleasedDate = releaseNote.ReleasedDate,
                    IsActive = true,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString()
                };
                var entity = _dbContext.ReleaseNotes.FirstOrDefault(x => x.ProjectId == releaseNote.ProjectId && x.IsActive == true);

                if (entity != null)
                {
                    entity.IsActive = false;
                    _dbContext.ReleaseNotes.Update(entity);
                }
                await _dbContext.ReleaseNotes.AddAsync(releaseNotes);
                await _dbContext.SaveChangesAsync();

                foreach (var Id in releaseNote.TaskIdList)
                {
                    TaskReleaseNotes taskReleaseNotes = new TaskReleaseNotes()
                    {
                        ReleaseNotesId = releaseNotes.Id,
                        TaskId = Id,
                        ProjectId = releaseNote.ProjectId,
                        Version = releaseNote.Version,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString()
                    };

                    await _dbContext.TaskReleaseNotes.AddAsync(taskReleaseNotes);

                    var task = _dbContext.Task.FirstOrDefault(x => x.Id == Id);
                    task.Status = Status.InUAT;
                    task.UpdatedBy = user.Id.ToString();
                    task.UpdatedDate = DateTime.Now;

                    var employeeTask = _dbContext.EmployeeTask.Where(x => x.TaskId == Id).ToList();

                    foreach (var obj in employeeTask)
                    {
                        obj.Status = Status.InUAT;
                        obj.UpdatedBy = user.Id.ToString();
                        obj.UpdatedDate = DateTime.Now;
                        var employeeDailyTask = _dbContext.EmployeeDailyTask.FirstOrDefault(x => x.EmployeeTaskId == obj.Id);
                        employeeDailyTask.Status = Status.InUAT;
                        employeeDailyTask.UpdatedBy = user.Id.ToString();
                        employeeDailyTask.UpdatedDate = DateTime.Now;
                    }

                }

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReleaseNotes> GetReleaseNotes(int projectId)
        {
            var releaseNotes = _dbContext.ReleaseNotes.FirstOrDefault(x => x.ProjectId == projectId && x.IsActive == true);
            return releaseNotes;
        }

        public async Task<List<ReleaseNotesDto>> GetReleaseNotesHistory(int projectId)
        {
            var releaseNotes = _dbContext.ReleaseNotes.Where(x => x.ProjectId == projectId).ToList();
            List<ReleaseNotesDto> releaseNotesHistory = new List<ReleaseNotesDto>();

            foreach (var notes in releaseNotes)
            {
                var taskReleaseNotes = _dbContext.TaskReleaseNotes.Where(x => x.ReleaseNotesId == notes.Id).ToList();
                List<Data.Model.Task> tasks = new List<Data.Model.Task>();

                foreach(var taskNotes in taskReleaseNotes)
                {
                    var task = _dbContext.Task.FirstOrDefault(x => x.Id == taskNotes.TaskId);
                    tasks.Add(task);
                }

                ReleaseNotesDto releaseNotesDto = new ReleaseNotesDto()
                {
                    Description = notes.Description,
                    ReleasedDate = notes.ReleasedDate,
                    Version = notes.Version,
                    Tasks = tasks
                };
                releaseNotesHistory.Add(releaseNotesDto);
            }

            return releaseNotesHistory;
        }
    }
}
