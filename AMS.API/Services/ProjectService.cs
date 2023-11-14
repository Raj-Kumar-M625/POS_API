using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Constants;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public ProjectService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<List<Project>> GetAllProjectlist()
        {
            try
            {
                var projectList = await _repository.Project.FindAllAsync();
                var result = projectList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetEmployeeProjectlist(User user)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var employeeProject = await _repository.EmployeeProject.GetEmployeeProjectList(employee.Id);
                var projectList = await _repository.Project.FindByConditionAsync(x => employeeProject.Contains(x.Id));

                foreach (var obj in projectList)
                {
                    var techStack = await _repository.ProjectTechStack.FindByConditionAsync(x => x.ProjectId == obj.Id);
                    obj.ProjectTechStacks = techStack.ToArray();
                }
                var result = projectList.ToList();
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
        public async Task<bool> AddEmployeeProject(User user, EmployeeProjectDto employeeProject)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                EmployeeProject empProject = new()
                {
                    ProjectId = employeeProject.ProjectId,
                    EmployeeId = employeeProject.EmployeeId,
                    CreatedBy = employee.UserId.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = employee.UserId.ToString(),
                };
                var employeeLoginDetails = await _repository.EmployeeProject.CreateAsync(empProject);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<List<UserInterface>> GetUserInterfacelist(int projectid)
        {
            try
            {
                var documents = await _dbContext.Document.ToListAsync();
                var UIList = _dbContext.UserInterface.Where(x => x.ProjectId == projectid).OrderByDescending(x => x.Id).ToList();
                var result = UIList.ToList();
                foreach (var obj in result)
                {
                    var _document = documents.Where(x => x.AttributeId == obj.Id).ToList();
                    obj.Documents = _document;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserInterface> AddUserInterface(User user, UserInterface userInterface)
        {
            try
            {
                UserInterface userInterface1 = new UserInterface()
                {
                    ProjectId = userInterface.ProjectId,
                    ProjectObjectiveId = userInterface.ProjectObjectiveId,
                    Name = userInterface.Name,
                    Description = userInterface.Description,
                    Status = userInterface.Status,
                    Complexity = userInterface.Complexity,
                    Percentage = userInterface.Percentage,
                    StartDate = userInterface.StartDate,
                    EndDate = userInterface.EndDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id.ToString(),
                    UpdatedBy = user.Id.ToString(),
                    UICategory = userInterface.UICategory,
                };
                var UI = await _dbContext.UserInterface.AddAsync(userInterface1);
                await _dbContext.SaveChangesAsync();
                return UI.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUserInterface(UserInterface userInterface)
        {
            try
            {
                var userInterface1 = await _dbContext.UserInterface.FirstOrDefaultAsync(x => x.Id == userInterface.Id);

                if (userInterface1 != null)
                {
                    userInterface1.ProjectId = userInterface1.ProjectId;
                    userInterface1.ProjectObjectiveId = userInterface.ProjectObjectiveId;
                    userInterface1.Name = userInterface.Name;
                    userInterface1.Description = userInterface.Description;
                    userInterface1.UICategory = userInterface.UICategory;
                    userInterface1.Status = userInterface.Status;
                    userInterface1.Complexity = userInterface.Complexity;
                    userInterface1.Percentage = userInterface1.Percentage;
                    userInterface1.StartDate = userInterface.StartDate;
                    userInterface1.EndDate = userInterface.EndDate;
                    userInterface1.UpdatedDate = DateTime.Now;
                }
                else
                {
                    return false;
                };

                _dbContext.UserInterface.Update(userInterface1);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmployeeTask> CreateEmployeeDayPlan(User user, EmployeeTaskDto dayPlan)
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
                var EmployeeDayPlan = _mapper.Map<EmployeeTask>(dayPlan);
                //EmployeeDayPlan.Status = "Pending";
                EmployeeDayPlan.CreatedBy = user.Id.ToString();
                EmployeeDayPlan.UpdatedBy = user.Id.ToString();
                EmployeeDayPlan.EstStartDate = (DateTime)dayPlan.StartDate;
                EmployeeDayPlan.EstEndDate = (DateTime)dayPlan.EndDate;
                EmployeeDayPlan.CreatedDate = DateTime.Now;
                EmployeeDayPlan.WeekEndingDate = weekEndingDate;
                var taskcreated = await _repository.EmployeeTask.CreateAsync(EmployeeDayPlan);


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

        public async Task<List<ProjectDto>> GetProjectlist(User sessionUser)
        {
            try
            {
                var projectList = (from project in _dbContext.Project
                                   select new ProjectDto()
                                   {
                                       Id = project.Id,
                                       Name = project.Name,
                                       Type = project.Type,
                                       Description = project.Description,
                                       Status = project.Status,
                                       Percentage = (int)project.Percentage,
                                       StartDate = project.StartDate,
                                       EndDate = project.EndDate,
                                       ProjectDocuments = _dbContext.ProjectDocuments.FirstOrDefault(x => x.ProjectId == project.Id),
                                       ProjectTechStacks = _dbContext.ProjectTechStack.Where(x => x.ProjectId == project.Id).ToList(),
                                       UserStoryCount = _dbContext.UserStory.Where(x => x.ProjectId == project.Id).Count(),
                                       InProgressCount = _dbContext.Task.Where(x => x.Status == "Pending" && x.ProjectId == project.Id).Count(),
                                       TotalTaskCount = _dbContext.Task.Where(x => x.ProjectId == project.Id).Count(),
                                       NotStartedTaskCounts = _dbContext.Task.Where(x => x.ProjectId == project.Id && x.Status == "Unassigned").Count(),
                                       UseInterfaceCount = _dbContext.UserInterface.Where(x => x.ProjectId == project.Id).Count(),
                                       TeamId = _dbContext.TeamProject.Where(x => x.ProjectId == project.Id && x.EndDate == null).Select(x => x.TeamId).FirstOrDefault(),
                                   }).OrderByDescending(x => x.Id).ToList();
                if (sessionUser.UserType == "Customer")
                {
                    var employee = await _dbContext.Employee.FirstOrDefaultAsync(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == employee.Id).ToListAsync();
                    projectList = projectList.Where(x => customerProject.Any(cp => cp.ProjectId == x.Id)).ToList();
                }
                return projectList;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Project> GetProjectById(int Id)
        {
            var project = await _dbContext.Project.Include(o => o.ProjectDocuments)
            .Include(p => p.ProjectTechStacks).ThenInclude(o => o.CommonMaster)
            .ToListAsync();
            var projectValue = project.Where(x => x.Id == Id).First();
            return projectValue;
        }
        public async Task<bool> AddProject(User user, Project project)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                Project myProject = new()
                {
                    Name = project.Name,
                    Type = project.Type,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status,
                    Percentage = project.Percentage,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedBy = employee.UserId.ToString(),
                };
                var projectDetails = await _repository.Project.CreateAsync(myProject);
                foreach (var Id in project.TechStackId)
                {
                    ProjectTechStack myTechSrack = new()
                    {
                        ProjectId = projectDetails.Id,
                        TechStack = Id,
                        CreatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = employee.UserId.ToString(),
                    };
                    var employeeLoginDetails = await _repository.ProjectTechStack.CreateAsync(myTechSrack);
                }
                if (project.TeamId > 0)
                {
                    TeamProject teamProject = new()
                    {
                        ProjectId = projectDetails.Id,
                        TeamId = project.TeamId,
                        CreatedBy = user.Id.ToString(),
                        StartDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = employee.UserId.ToString(),
                    };
                    var addTeamProject = await _repository.TeamProject.CreateAsync(teamProject);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateProject(User user, Project project, int Id)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var projectValue = await _dbContext.Project.Include(o => o.ProjectDocuments)
                    .Include(p => p.ProjectTechStacks).ThenInclude(o => o.CommonMaster)
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (projectValue != null)
                {
                    projectValue.Name = project.Name;
                    projectValue.Type = project.Type;
                    projectValue.Description = project.Description;
                    projectValue.StartDate = project.StartDate;
                    projectValue.EndDate = project.EndDate;
                    projectValue.Status = project.Status;
                    projectValue.Percentage = project.Percentage;
                    projectValue.UpdatedDate = DateTime.Now;
                    projectValue.UpdatedBy = employee.UserId.ToString();
                    await _repository.Project.UpdateAsync(projectValue);
                }
                else
                {
                    return false;
                }
                if (project.TeamId > 0)
                {
                    var teamProject = _dbContext.TeamProject.Where(x => x.ProjectId == Id && x.EndDate == null).FirstOrDefault();
                    if (teamProject != null)
                    {
                        var teamProjectValue = teamProject.TeamId == project.TeamId;
                        if (teamProjectValue != true)
                        {
                            teamProject.EndDate = DateTime.Now;
                            teamProject.UpdatedDate = DateTime.Now;
                            teamProject.UpdatedBy = employee.UserId.ToString();
                            var updateTemproject = await _repository.TeamProject.UpdateAsync(teamProject);
                        }

                        if (teamProjectValue != true)
                        {
                            TeamProject teamProject1 = new()
                            {
                                ProjectId = projectValue.Id,
                                TeamId = project.TeamId,
                                CreatedBy = user.Id.ToString(),
                                StartDate = DateTime.Now,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = employee.UserId.ToString(),
                            };
                            var addTeamProject = await _repository.TeamProject.CreateAsync(teamProject1);

                        }


                    }
                }

                var projectTech = await _repository.ProjectTechStack.FindAllAsync();
                var projectTechVal = projectTech.Where(x => x.ProjectId == projectValue.Id);
                var techIdsToDelete = projectTechVal.Where(x => !project.TechStackId.Contains(x.TechStack)).Select(x => x.Id).ToList();

                foreach (var techIdToDelete in techIdsToDelete)
                {
                    var projectTechToDelete = projectTech.FirstOrDefault(x => x.Id == techIdToDelete);

                    if (projectTechToDelete != null)
                    {
                        await _repository.ProjectTechStack.DeleteAsync(projectTechToDelete);
                    }
                }


                foreach (var techId in project.TechStackId)
                {
                    var existingProjectTech = projectTech.FirstOrDefault(x => x.TechStack == techId && x.ProjectId == projectValue.Id);

                    if (existingProjectTech != null)
                    {
                        existingProjectTech.UpdatedDate = DateTime.Now;
                        existingProjectTech.UpdatedBy = employee.UserId.ToString();
                        await _repository.ProjectTechStack.UpdateAsync(existingProjectTech);
                    }
                    else
                    {
                        ProjectTechStack newProjectTech = new()
                        {
                            ProjectId = projectValue.Id,
                            TechStack = techId,
                            CreatedBy = user.Id.ToString(),
                            CreatedDate = DateTime.Now,
                            UpdatedBy = employee.UserId.ToString(),
                        };
                        await _repository.ProjectTechStack.CreateAsync(newProjectTech);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<ProjectObjectiveDto> GetProjectObjective(int ProjectId)
        {
            try
            {
                var projectObjectiveMapping = await _dbContext.ProjectObjectiveMapping.Where(x => x.ProjectId == ProjectId).ToListAsync();
                var projectObjective = await _dbContext.ProjectObjective.Where(x => x.ProjectId == ProjectId).ToListAsync();
                ProjectObjectiveDto projectObjectiveDto = new ProjectObjectiveDto()
                {
                    ProjectObjectives = projectObjective,
                    ProjectObjectiveMappings = projectObjectiveMapping
                };
                return projectObjectiveDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ProjectObjective> GetProjectObjectiveById(int Id)
        {
            var projectObjective = await _dbContext.ProjectObjective
        .SingleOrDefaultAsync(x => x.Id == Id);
            return projectObjective;
        }


        public async Task<bool> AddProjectObjective(User user, ProjectObjective projectObjective)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                ProjectObjective myProjectObjective = new()
                {
                    ProjectId = projectObjective.ProjectId,
                    Description = projectObjective.Description,
                    Status = projectObjective.Status,
                    Percentage = projectObjective.Percentage,
                    CreatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = employee.UserId.ToString(),
                };
                await _repository.ProjectObjective.CreateAsync(myProjectObjective);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateProjectObjective(User user, ProjectObjective project, int Id)
        {
            try
            {

                var projectobjective = await _dbContext.ProjectObjective
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (projectobjective != null)
                {
                    projectobjective.Description = project.Description;
                    projectobjective.Status = project.Status;
                    projectobjective.Percentage = projectobjective.Percentage;
                    projectobjective.CreatedDate = projectobjective.CreatedDate;
                    projectobjective.CreatedBy = projectobjective.CreatedBy;
                    projectobjective.UpdatedDate = DateTime.Now;
                    projectobjective.UpdatedBy = user.Id.ToString();
                    await _repository.ProjectObjective.UpdateAsync(projectobjective);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<UserStory>> GetUserStoryList(int projectId)
        {
            try
            {
                var documents = await _dbContext.Document.ToListAsync();
                var UserStory = await _dbContext.UserStory.Where(x => x.ProjectId == projectId).OrderByDescending(x => x.Id).ToListAsync();
                foreach (var obj in UserStory)
                {
                    var _document = documents.Where(x => x.AttributeId == obj.Id).ToList();
                    obj.Documents = _document; 
                }
                return UserStory;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<UserStory> AddUserStory(User user, UserStory UserStory)
        {
            try
            {
                UserStory MyUserStory = new UserStory()
                {
                    Name = UserStory.Name,
                    Description = UserStory.Description,
                    Status = UserStory.Status,
                    Percentage = UserStory.Percentage,
                    ProjectId = UserStory.ProjectId,
                    ProjectObjectiveId = UserStory.ProjectObjectiveId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id.ToString(),
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = user.Id.ToString(),
                };
                var entity = await _dbContext.UserStory.AddAsync(MyUserStory);
                await _dbContext.SaveChangesAsync();

                foreach (var id in UserStory.ProjectObjectiveIds)
                {
                    var projectObjective = _dbContext.ProjectObjective.FirstOrDefault(x => x.Id == id);
                    ProjectObjectiveMapping projectObjectiveMapping = new ProjectObjectiveMapping()
                    {
                        ProjectObjectiveId = id,
                        UserStoryId = entity.Entity.Id,
                        ProjectId = UserStory.ProjectId,
                        Description = projectObjective.Description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id.ToString(),
                        UpdatedDate = DateTime.Now,
                        UpdatedBy = user.Id.ToString(),
                    };
                    await _dbContext.ProjectObjectiveMapping.AddAsync(projectObjectiveMapping);
                }
                await _dbContext.SaveChangesAsync();

                return entity.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UploadFiles(User user, Document document)
        {
            try
            {
                var path = _configuration.GetValue<string>("FilePath");
                if (document.File != null && document.File.Length > 0)
                {
                    var filePath = Path.Combine(path, document.File.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.File.CopyTo(stream);
                    }
                }
                document.FilePath = Path.Combine(path, document.File.FileName);
                document.CreatedBy = user.Id.ToString();
                document.UpdatedBy = user.Id.ToString();
                document.CreatedDate = DateTime.Now;
                document.UpdatedDate = DateTime.Now;
                await _dbContext.Document.AddAsync(document);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Document> DownloadFile(int id)
        {
            var document = _dbContext.Document.FirstOrDefault(x => x.Id == id);
            string filePath = document.FilePath;
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] fileContent = File.ReadAllBytes(filePath);
                    document.FileContent = fileContent;
                    return document;
                }
                catch (IOException e)
                {
                    throw e;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteFile(int id)
        {
            try
            {
                var document = _dbContext.Document.FirstOrDefault(x => x.Id == id);
                string filePath = document.FilePath;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _dbContext.Document.Remove(document);
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
                throw ex;
            }

        }
        public async Task<bool> UpdateUserStory(User user, UserStory UserStory)
        {
            try
            {
                var MyUserStory = await _dbContext.UserStory
                    .FirstOrDefaultAsync(x => x.Id == UserStory.Id);

                var mappedObjective = await _dbContext.ProjectObjectiveMapping.Where(x => x.UserStoryId == UserStory.Id).ToListAsync();
                if (MyUserStory != null)
                {
                    MyUserStory.Name = UserStory.Name;
                    MyUserStory.Description = UserStory.Description;
                    MyUserStory.Status = UserStory.Status;
                    MyUserStory.Percentage = UserStory.Percentage;
                    MyUserStory.ProjectObjectiveId = UserStory.ProjectObjectiveId;
                    MyUserStory.StartDate = UserStory.StartDate;
                    MyUserStory.EndDate = UserStory.EndDate;
                    MyUserStory.UpdatedDate = DateTime.Now;
                    MyUserStory.UpdatedBy = UserStory.UpdatedBy;
                    _dbContext.UserStory.Update(MyUserStory);
                    await _dbContext.SaveChangesAsync();

                    if (mappedObjective.Count > 0)
                    {
                        _dbContext.ProjectObjectiveMapping.RemoveRange(mappedObjective);
                        await _dbContext.SaveChangesAsync();

                        foreach (var id in UserStory.ProjectObjectiveIds)
                        {
                            var projectObjective = _dbContext.ProjectObjective.FirstOrDefault(x => x.Id == id);
                            ProjectObjectiveMapping projectObjectiveMapping = new ProjectObjectiveMapping()
                            {
                                ProjectObjectiveId = id,
                                UserStoryId = UserStory.Id,
                                ProjectId = UserStory.ProjectId,
                                Description = projectObjective.Description,
                                CreatedDate = DateTime.Now,
                                CreatedBy = user.Id.ToString(),
                                UpdatedDate = DateTime.Now,
                                UpdatedBy = user.Id.ToString(),
                            };
                            await _dbContext.ProjectObjectiveMapping.AddAsync(projectObjectiveMapping);
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddUserStoryUI(User user, UserStoryUI[] userStoryUI)
        {
            try
            {
                var result = _dbContext.UserStoryUI.Where(x => x.UserStoryId == userStoryUI[0].UserStoryId).ToList();
                _dbContext.UserStoryUI.RemoveRange(result);

                foreach (var obj in userStoryUI)
                {
                    var userstoryUI = new UserStoryUI()
                    {
                        UIId = obj.UIId,
                        UserStoryId = obj.UserStoryId,
                        CreatedBy = user.Id.ToString(),
                        UpdatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    await _dbContext.UserStoryUI.AddAsync(userstoryUI);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<UserStoryIdDto>> GetUserStoryUIList(int userStoryId)
        {
            try
            {
                var result = _dbContext.UserStoryUI.Where(x => x.UserStoryId == userStoryId).ToList();

                var UserInterfacelistUI = (from userstoryui in _dbContext.UserStoryUI
                                           join userstory in _dbContext.UserStory on userstoryui.UserStoryId equals userstory.Id
                                           join userinterface in _dbContext.UserInterface on userstoryui.UIId equals userinterface.Id
                                           where userstoryui.UserStoryId == userStoryId
                                           select new UserStoryIdDto()
                                           {
                                               userStoryId = userstoryui.UserStoryId,
                                               userStoryName = userstory.Name,
                                               UIid = userstoryui.UIId,
                                               name = userinterface.Name,
                                               description = userinterface.Description,
                                               percentage = userinterface.Percentage,
                                               complexity = userinterface.Complexity,
                                               uiCategory = userinterface.UICategory,
                                               status = userinterface.Status,
                                               startDate = userinterface.StartDate,
                                               endDate = userinterface.EndDate,

                                           }).ToList();

                return UserInterfacelistUI;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AssignEmployeeProject(User user, EmployeeProject[] employeeProject)
        {
            var projempList = _dbContext.EmployeeProject.Where(x => x.ProjectId == employeeProject[0].ProjectId).ToList();

            foreach (var obj in employeeProject)
            {
                EmployeeProject emp = projempList.FirstOrDefault(x => x.EmployeeId == obj.EmployeeId && x.EndDate == null);

                if (emp == null)
                {
                    EmployeeProject employeeProject1 = new EmployeeProject()
                    {
                        ProjectId = obj.ProjectId,
                        EmployeeId = obj.EmployeeId,
                        StartDate = DateTime.Now,
                        CreatedBy = user.Id.ToString(),
                        UpdatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,

                    };
                    await _dbContext.EmployeeProject.AddAsync(employeeProject1);
                }
            }

            foreach (var obj in projempList)
            {
                var emp = employeeProject.FirstOrDefault(x => x.EmployeeId == obj.EmployeeId && obj.EndDate == null);

                if (emp == null)
                {
                    if (obj.EndDate == null)
                    {
                        obj.EndDate = DateTime.Now;
                        _dbContext.EmployeeProject.Update(obj);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignLead(User user, int userId, int projectId)
        {
            try
            {

                var result = await _dbContext.Lead.FirstOrDefaultAsync(x => x.UserId == userId);
                var empproject = await _dbContext.EmployeeProject.Where(x => x.ProjectId == projectId).ToListAsync();

                if (result == null)
                {
                    Lead lead = new Lead()
                    {
                        UserId = userId,
                        StartDate = DateTime.Now,
                        CreatedBy = user.Id.ToString(),
                        UpdatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,
                    };

                    var res = await _dbContext.Lead.AddAsync(lead);
                    await _dbContext.SaveChangesAsync();

                    foreach (var obj in empproject)
                    {
                        obj.LeadId = res.Entity.Id;
                    }

                    _dbContext.EmployeeProject.UpdateRange(empproject);
                }
                else
                {
                    foreach (var obj in empproject)
                    {
                        obj.LeadId = result.Id;
                    }

                    _dbContext.EmployeeProject.UpdateRange(empproject);

                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetUnAssignedProjects(int TeamID)
        {

            var Ids = _dbContext.TeamProject.Where(x => x.TeamId == TeamID).Select(x => x.ProjectId).ToList();
            var project = _dbContext.Project
                 .Where(x => !Ids.Contains(x.Id))
                .ToList();
            return project;
        }

        public async Task<List<UserStory>> GetAllUserStory(int projectId)
        {
            try
            {
                var userStory = await _dbContext.UserStory.Where(x => x.ProjectId == projectId).ToListAsync();

                var userInterfaceList = (from userStoryUi in _dbContext.UserStoryUI
                                         join userInterface in _dbContext.UserInterface on userStoryUi.UIId equals userInterface.Id
                                         select new UserStoryUIDto()
                                         {
                                             UIId = userInterface.Id,
                                             UserStoryId = userStoryUi.UserStoryId,
                                             UIName = userInterface.Name,
                                             Status = userInterface.Status
                                         }).ToList();

                foreach (var obj in userStory)
                {
                    var uiList = userInterfaceList.Where(x => x.UserStoryId == obj.Id).ToList();
                    obj.userStoryUIs = uiList;
                }

                return userStory;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProjectStatDto> GetProjectStatDetails(int ProjectId)
        {
            try
            {
                var totalTask = await _repository.Task.FindByConditionAsync(x => x.ProjectId == ProjectId);
                var inProgressTask = await _repository.Task.FindByConditionAsync(x => x.ProjectId == ProjectId && x.Status == "In-Progress");
                var completedTask = await _repository.Task.FindByConditionAsync(x => x.ProjectId == ProjectId && x.Status == "Completed");
                var totalUserStory = await _repository.UserStory.FindByConditionAsync(x => x.ProjectId == ProjectId);
                var totalUI = await _repository.UserInterface.FindByConditionAsync(x => x.ProjectId == ProjectId);
                var totalProjObj = await _repository.ProjectObjective.FindByConditionAsync(x => x.ProjectId == ProjectId);
                var result = inProgressTask.ToList();

                var teamDetails = (from teamProj in _dbContext.TeamProject
                                   join team in _dbContext.Team on teamProj.TeamId equals team.Id
                                   where teamProj.ProjectId == ProjectId
                                   select new ProjectStatDto
                                   {
                                       TeamName = team.Name,
                                   }).FirstOrDefault();

                ProjectStatDto projStat = new()
                {
                    InProgressTask = inProgressTask.Count(),
                    TotalTask = totalTask.Count(),
                    CompletedTask = completedTask.Count(),
                    TotalUserStory = totalUserStory.Count(),
                    TotalUI = totalUI.Count(),
                    TotalProjectObjective = totalProjObj.Count(),
                    TeamName = teamDetails?.TeamName
                };
                // Calculate the percentage for each table based on the Percentage column
                projStat.TaskPercentage = totalTask.Count() != 0 ? totalTask.Sum(task => task.Percentage) / totalTask.Count() : 0;
                projStat.UserStoryPercentage = totalUserStory.Count() != 0 ? totalUserStory.Sum(x => x.Percentage) / totalUserStory.Count() : 0;
                projStat.UIPercentage = totalUI.Count() != 0 ? totalUI.Sum(x => x.Percentage) / totalUI.Count() : 0;
                projStat.ObjectivePercentage = totalProjObj.Count() != 0 ? totalProjObj.Sum(x => x.Percentage) / totalProjObj.Count() : 0;
                projStat.InProgressPercentage = inProgressTask.Count() != 0 ? inProgressTask.Sum(x => x.Percentage) / inProgressTask.Count() : 0;
                projStat.CompletedPercentage = completedTask.Count() != 0 ? completedTask.Sum(x => x.Percentage) / completedTask.Count() : 0;


                return projStat;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ProjectDashboardDto> getProjectDashBoardData()
        {
            try
            {
                var userStory = await _dbContext.UserStory.ToListAsync();
                var commonUserStory = new CommonUserStory()
                {
                    NotStarted = userStory.FindAll(x => x.Percentage == 0).Count,
                    Completed = userStory.FindAll(x => x.Percentage == 100).Count,
                    Pending = userStory.FindAll(x => x.Percentage < 100 && x.Percentage > 0).Count,
                };

                var userInterface = await _dbContext.UserInterface.ToListAsync();
                var CommonUserInterface = new CommonUserInterface()
                {
                    NotStarted = userInterface.FindAll(x => x.Percentage == 0).Count,
                    Completed = userInterface.FindAll(x => x.Percentage == 100).Count,
                    Pending = userInterface.FindAll(x => x.Percentage < 100 && x.Percentage > 0).Count,
                };

                var task = await _dbContext.Task.ToListAsync();
                var commonTask = new CommonTask()
                {
                    Completed = task.FindAll(x => x.Status == "Completed").Count,
                    Assigned = task.FindAll(x => x.Status == "Assigned").Count,
                    ReadyForUAT = task.FindAll(x => x.Status == "Ready For UAT" || x.Status == "Ready-For-UAT").Count,
                    Unassigned = task.FindAll(x => x.Status == "Unassigned").Count,
                    InProgress = task.FindAll(x => x.Status == "In-Progress").Count,
                };

                var project = await _dbContext.Project.ToListAsync();
                var commonProject = new CommonProject()
                {
                    Completed = project.FindAll(x => x.Percentage == 100).Count,
                    OnGoing = project.FindAll(x => x.Percentage < 100 && x.Percentage > 0).Count,
                    NotStarted = project.FindAll(x => x.Percentage == 0).Count,
                };

                var userStories = await _dbContext.UserStory.ToListAsync();
                var userInterfaces = await _dbContext.UserInterface.ToListAsync();

                var ProjectDashboardDto = new ProjectDashboardDto()
                {
                    CommonProject = commonProject,
                    CommonTask = commonTask,
                    CommonUserInterface = CommonUserInterface,
                    CommonUserStory = commonUserStory,
                    Projects = _dbContext.Project.Include(o => o.ProjectTechStacks).ThenInclude(o => o.CommonMaster).ToList(),
                    UserStories = userStories,
                    UserInterface = userInterfaces
                };
                return ProjectDashboardDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProjectCatergory> getEachProjectCategories(int projectId)
        {
            try
            {
                var ProjectCategories = await (from task in _dbContext.Task
                                               join category in _dbContext.Category on task.CategoryId equals category.Id
                                               join project in _dbContext.Project on task.ProjectId equals project.Id
                                               join employee in _dbContext.Employee on task.CreatedBy equals employee.UserId.ToString()
                                               where task.ProjectId == projectId
                                               select new ProjectCatergory
                                               {
                                                   Id = task.ProjectId,
                                                   Name = project.Name,
                                               }).FirstOrDefaultAsync();

                if (ProjectCategories != null)
                {
                    var distinctCreatedByCount = await (from task in _dbContext.Task
                                                        join employee in _dbContext.Employee on task.CreatedBy equals employee.UserId.ToString()
                                                        where employee.Department == "Development"
                                                        where task.ProjectId == projectId
                                                        select task.CreatedBy).Distinct().CountAsync();

                    var distinctCreatedByBACount = await (from task in _dbContext.Task
                                                          join employee in _dbContext.Employee on task.CreatedBy equals employee.UserId.ToString()
                                                          where employee.Department == "BusinessAnalyst"
                                                          where task.ProjectId == projectId
                                                          select task.CreatedBy).Distinct().CountAsync();

                    var distinctCreatedByQACount = await (from task in _dbContext.Task
                                                          join employee in _dbContext.Employee on task.CreatedBy equals employee.UserId.ToString()
                                                          where employee.Department == "Testing"
                                                          where task.ProjectId == projectId
                                                          select task.CreatedBy).Distinct().CountAsync();

                    var ProjectCatergory = new ProjectCatergory()
                    {
                        Id = ProjectCategories.Id,
                        Name = ProjectCategories.Name,
                        TotalDevelopment = distinctCreatedByCount,
                        TotalBusinessAnalyst = distinctCreatedByBACount,
                        TotalTesting = distinctCreatedByQACount
                    };

                    return ProjectCatergory;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProjectCheckList>> getCheckListProject()
        {
            try

            {
                var projectList = await _dbContext.Project.ToListAsync();
                var userTaskCheckList = await _dbContext.UserTaskCheckList.ToListAsync();
                List<ProjectCheckList> projectCheckLists = new List<ProjectCheckList>();

                foreach (var obj in projectList)
                {
                    var taskCheckList = userTaskCheckList.Where(x => x.ProjectId == obj.Id).ToList();

                    if (taskCheckList.Count > 0)
                    {

                        ProjectCheckList projectCheckList = new ProjectCheckList()
                        {
                            ProjectName = obj.Name,
                            ProjectId = obj.Id,
                            DevCheckCount = taskCheckList.Where(x => x.IsDevChecked == true).Count(),
                            QACheckCount = taskCheckList.Where(x => x.IsQAChecked == true).Count()
                        };

                        projectCheckLists.Add(projectCheckList);
                    }
                }
                return projectCheckLists;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ProjectDto>> GetAllProjectlists(AttendenceFilterDto Tasklist)
        {
            try
            {


                var employeeProject = await _repository.EmployeeProject.FindByConditionAsync(x => x.EmployeeId == Tasklist.employeeId);
                var employeeprojects = employeeProject.Select(x => x.ProjectId).ToList();
                var projectList = await _repository.Project.FindByConditionAsync(x => employeeprojects.Contains(x.Id));

                var projectDtos = new List<ProjectDto>();
                int? totalTaskCount = 0;
                int? totalCompletedCount = 0;
                int? totalInProgressCount = 0;
                foreach (var project in projectList)
                {
                    var projectDto = new ProjectDto
                    {
                        ProjectName = project.Name,
                        Id = project.Id
                    };

                    if (Tasklist.months != null)
                    {
                        DateTime currentDate = DateTime.Now;
                        DateTime startDate = new DateTime(currentDate.Year, Tasklist.months[0], 1);
                        DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                        IEnumerable<EmployeeTask> tasksForProject = new List<EmployeeTask>();
                        if (Tasklist.weekendingDate.Count() > 0 && Tasklist.status.Count() > 0)
                        {
                            tasksForProject = await _repository.EmployeeTask.FindByConditionAsync(x => x.ProjectId == project.Id && x.EmployeeId == Tasklist.employeeId && x.EstStartDate.Date >= startDate.Date && x.EstStartDate.Date <= endDate.Date && Tasklist.status.Contains(x.Status) && Tasklist.weekendingDate.Contains(x.WeekEndingDate.Date.ToString()));

                        }
                        else if (Tasklist.weekendingDate.Count() > 0 && Tasklist.status.Count() == 0)
                        {
                            tasksForProject = await _repository.EmployeeTask.FindByConditionAsync(x => x.ProjectId == project.Id && x.EmployeeId == Tasklist.employeeId && x.EstStartDate.Date >= startDate.Date && x.EstStartDate.Date <= endDate.Date && Tasklist.weekendingDate.Contains(x.WeekEndingDate.Date.ToString()));

                        }
                        else if (Tasklist.weekendingDate.Count() == 0 && Tasklist.status.Count() > 0)
                        {
                            tasksForProject = await _repository.EmployeeTask.FindByConditionAsync(x => x.ProjectId == project.Id && x.EmployeeId == Tasklist.employeeId && x.EstStartDate.Date >= startDate.Date && x.EstStartDate.Date <= endDate.Date && Tasklist.status.Contains(x.Status));

                        }
                        else
                        {
                            tasksForProject = await _repository.EmployeeTask.FindByConditionAsync(x => x.ProjectId == project.Id && x.EmployeeId == Tasklist.employeeId && x.EstStartDate.Date >= startDate.Date && x.EstStartDate.Date <= endDate.Date);

                        }
                        projectDto.AssignedHours = tasksForProject.Sum(x => x.EstTime);
                        projectDto.AssignedTaskCount = tasksForProject.Count();
                        projectDto.ProjectCount = employeeprojects.Count();
                        totalTaskCount += projectDto.AssignedTaskCount;
                        totalCompletedCount += tasksForProject.Count(x => x.Status == "Completed");
                        totalInProgressCount += tasksForProject.Count(x => x.Status == "In-Progress");
                        projectDto.TotalTaskCount = totalTaskCount;
                        projectDto.TotalInProgressCount = totalInProgressCount;
                        projectDto.TotalCompletedCount = totalCompletedCount;
                        projectDto.TotalTaskCount = totalTaskCount;
                        projectDto.condition = Tasklist.status;
                        projectDto.weekenddate = Tasklist.weekendingDate;
                        projectDto.month = Tasklist.months;
                        projectDtos.Add(projectDto);
                    }
                    else
                    {
                        IEnumerable<EmployeeTask> tasksForProject = new List<EmployeeTask>();
                        if (Tasklist.status != null)
                        {
                            DateTime currentDate = DateTime.Now.Date;
                            tasksForProject = await _repository.EmployeeTask.FindByConditionAsync(x => x.ProjectId == project.Id && x.EmployeeId == Tasklist.employeeId && x.EstStartDate.Date == currentDate && Tasklist.status.Contains(x.Status));
                        }
                        tasksForProject = await _repository.EmployeeTask.FindByConditionAsync(x => x.ProjectId == project.Id && x.EmployeeId == Tasklist.employeeId && x.EstStartDate.Date == DateTime.Now);
                        projectDto.AssignedHours = tasksForProject.Sum(x => x.EstTime);
                        projectDto.AssignedTaskCount = tasksForProject.Count();
                        projectDto.ProjectCount = employeeprojects.Count();
                        totalTaskCount += projectDto.AssignedTaskCount;
                        totalCompletedCount += tasksForProject.Count(x => x.Status == "Completed");
                        totalInProgressCount += tasksForProject.Count(x => x.Status == "In-Progress");
                        projectDto.TotalTaskCount = totalTaskCount;
                        projectDto.TotalInProgressCount = totalInProgressCount;
                        projectDto.TotalCompletedCount = totalCompletedCount;
                        projectDto.TotalTaskCount = totalTaskCount;
                        projectDto.condition = Tasklist.status;
                        projectDto.weekenddate = Tasklist.weekendingDate;
                        projectDto.month = Tasklist.months;
                        projectDtos.Add(projectDto);
                    }

                }
                return projectDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AssignCustomerProject(User user, CustomerProject[] customerProject)
        {
            var projcusList = _dbContext.CustomerProject.Where(x => x.ProjectId == customerProject[0].ProjectId).ToList();

            foreach (var obj in customerProject)
            {
                CustomerProject emp = projcusList.FirstOrDefault(x => x.EmployeeId == obj.EmployeeId && x.EndDate == null);

                if (emp == null)
                {
                    CustomerProject customerProject1 = new CustomerProject()
                    {
                        ProjectId = obj.ProjectId,
                        EmployeeId = obj.EmployeeId,
                        StartDate = DateTime.Now,
                        CreatedBy = user.Id.ToString(),
                        UpdatedBy = user.Id.ToString(),
                        CreatedDate = DateTime.Now,

                    };
                    await _dbContext.CustomerProject.AddAsync(customerProject1);
                }
            }

            foreach (var obj in projcusList)
            {
                var emp = customerProject.FirstOrDefault(x => x.EmployeeId == obj.EmployeeId && obj.EndDate == null);

                if (emp == null)
                {
                    if (obj.EndDate == null)
                    {
                        obj.EndDate = DateTime.Now;
                        _dbContext.CustomerProject.Update(obj);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ReportWithCounts>> getCustomerProjectReport(int projectId)
        {
            try
            {

                var UserStory = await (from userstory in _dbContext.UserStory
                                       join userstoryUi in _dbContext.UserStoryUI on userstory.Id equals userstoryUi.UserStoryId
                                       join Taskbuglist in _dbContext.Task on userstoryUi.Id equals Taskbuglist.UserStoryUIId
                                       where userstory.ProjectId == projectId
                                       select new ProjectReportList
                                       {
                                           projectId = projectId,
                                           UIid = userstoryUi.Id,
                                           userstory = userstory.Name,
                                           userInterfacelists = _dbContext.UserInterface.Where(x => userstoryUi.UIId == x.Id).ToList(),
                                           TasKList = _dbContext.Task
                                              .Where(x => userstoryUi.Id == x.UserStoryUIId)
                                              .Select(task => new TaskDTO
                                              {
                                                  UIUserStoryId = task.UserStoryUIId,
                                                  UserStoryId = task.UserStoryId,
                                                  Id = task.Id,
                                                  CategoryId = task.CategoryId,
                                                  Classification = task.Classification,
                                                  TaskDescription = task.Description,
                                                  Status = task.Status,
                                                  TaskType = task.TaskType,
                                                  CreatedBy = task.CreatedBy,
                                                  EmployeeName = (
                                                            from employee in _dbContext.Employee
                                                            join user in _dbContext.Users on employee.UserId equals user.Id
                                                            where employee.Id.ToString() == task.CreatedBy
                                                            select user.Name
                                                       ).FirstOrDefault(),
                                                  EstTime = task.EstTime,
                                                  ActTime = task.ActTime,
                                              })
                                              .ToList(),
                                       }).ToListAsync();

                var dataWithCounts = UserStory.GroupBy(item => item.UIid)
                .Select(group => new ReportWithCounts
                {
                    Report = group.First(),
                    UserInterfacelistsCount = group.Select(item => item.userInterfacelists).Sum(list => list.Count),
                    TasKListCount = group.Select(item => item.TasKList).Sum(list => list.Count),
                })
         .ToList();

                return dataWithCounts;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProjectReportVM> GetProjectReportDetails(int projectId)
        {
            try
            {
                var query =await (from task in _dbContext.Task
                            where task.ProjectId == projectId
                            join userStory in _dbContext.UserStory on task.UserStoryId equals userStory.Id into userStoryGroup
                            from userStory in userStoryGroup.DefaultIfEmpty()
                            join userInterface in _dbContext.UserInterface on task.UserStoryUIId equals userInterface.Id into userInterfaceGroup
                            from userInterface in userInterfaceGroup.DefaultIfEmpty()
                            join category in _dbContext.Category on task.CategoryId equals category.Id into categoryGroup
                            from category in categoryGroup.DefaultIfEmpty()
                            join team in _dbContext.Team on task.TeamId equals team.Id into teamGroup
                            from team in teamGroup.DefaultIfEmpty()
                            join employeeTask in _dbContext.EmployeeTask on task.Id equals employeeTask.TaskId into employeeTaskGroup
                            from employeeTask in employeeTaskGroup.DefaultIfEmpty()
                            join employee in _dbContext.Employee on employeeTask.EmployeeId equals employee.Id into employeeGroup
                            from employee in employeeGroup.DefaultIfEmpty()
                            select new TaskListDto
                            {
                                Id = task.Id,
                                Name = task.Name,
                                Description = task.Description,
                                USName = userStory.Name,
                                UIName = userInterface.Name,
                                TeamName = team.Name,
                                EmployeeName = employee.Name,
                                Category = category.Categories,
                                SubCategory = category.SubCategory,
                                Status = task.Status,
                                Percentage = task.Percentage,
                                EstimateTime = task.EstTime,
                                ActualTime = task.ActTime,
                                Priority = task.Priority,
                                ActualStartDate = employeeTask.StartDate,
                                ActualEndDate = employeeTask.EndDate
                            }).ToListAsync();

                var priorityTask = query.Where(x => x.Priority == "High").ToList();
                var todaysTask = query.Where(x => x.ActualStartDate.HasValue && x.ActualStartDate.Value.Date == DateTime.Now.Date).ToList();
                var userStories = await _dbContext.UserStory.Where(x => x.ProjectId == projectId).ToListAsync();
                var userInterfaces = await _dbContext.UserInterface.Where(x => x.ProjectId == projectId).ToListAsync();
                var tasks = await _dbContext.Task.Where(x => x.ProjectId == projectId).ToListAsync();
                var totalResource = await _dbContext.EmployeeProject.Where(x => x.ProjectId == projectId).ToListAsync();
                var employeeTasks = await _dbContext.EmployeeTask.Where(x => x.ProjectId == projectId).ToListAsync();
                var resourceHours = employeeTasks.Where(x => totalResource.Any(tr => x.EmployeeId == x.EmployeeId)).Sum(x => x.ActTime);
                
                ProjectReportVM projectReportVM = new ProjectReportVM()
                {
                    HighPriorityTasks = priorityTask,
                    TodaysTasks = todaysTask,
                    UserStory = userStories,
                    UserInterface = userInterfaces,
                    Task = tasks,
                    TotalResource = totalResource.Count(),
                    TotalResourceHours = resourceHours,
                };

                var percenatge = SetPercentage(projectReportVM);
                projectReportVM.CompletedPercentage = percenatge[0];
                projectReportVM.PendingPercentage = percenatge[1];

                return projectReportVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<decimal> SetPercentage(ProjectReportVM projectReport)
        {
            int completed = 0;
            int pending = 0;
            int total = 0;

            total += projectReport.UserStory.Count;
            total += projectReport.UserInterface.Count;
            total += projectReport.Task.Count;

            completed += projectReport.UserStory.Count(x => x.Percentage == 100);
            completed += projectReport.UserInterface.Count(x => x.Percentage == 100);
            completed += projectReport.Task.Count(x => x.Percentage == 100);

            pending += projectReport.UserStory.Count(x => x.Percentage < 100);
            pending += projectReport.UserInterface.Count(x => x.Percentage < 100);
            pending += projectReport.Task.Count(x => x.Percentage < 100);

            List<decimal> list = new List<decimal>
            {
                (decimal)(completed * 100.0) / total,
                (decimal)(pending * 100.0) / total
            };

            return list;
        }

    }
}
