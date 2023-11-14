using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Services
{
    public class TeamService : ITeamService
    {

        private readonly ProjectOversightContext _dbContext;
        private readonly IUnitOfWork _repository;
        public TeamService(ProjectOversightContext dbContext, IUnitOfWork repository)
        {

            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<List<Team>> GetTeamList(User sessionUser)
        {
            try
            {
                var teamList = await _dbContext.Team.OrderByDescending(x => x.Id).ToListAsync();
                var teamProject = await _dbContext.TeamProject.ToListAsync();
                if (sessionUser.UserType == "Customer")
                {
                    var employee = await _dbContext.Employee.FirstOrDefaultAsync(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == employee.Id).ToListAsync();
                    teamProject = teamProject.Where(x => customerProject.Any(cp => cp.ProjectId == x.ProjectId)).ToList();
                    teamList = teamList.Where(x => teamProject.Any(tp => tp.TeamId == x.Id)).ToList();
                }
                return teamList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamEmployee>> GetTeamMemberList()
        {
            try
            {
                var teamList = await _dbContext.TeamEmployee.Include(o => o.Employee).Include(o => o.Employee.User).ToListAsync();
                return teamList;
            }
            catch (Exception ex)
            {
                throw;  
            }
        }
        public async Task<Team> GetTeamById(int Id)
        {
            try
            {
                var TeamList = await _dbContext.Team.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return TeamList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<TeamDto>> GetTeamNames(int projectId)
        {
            var query = await (
                               from employeeProject in _dbContext.EmployeeProject
                               join employee in _dbContext.Employee on employeeProject.EmployeeId equals employee.Id
                               join user in _dbContext.Users on employee.UserId equals user.Id
                               where employeeProject.ProjectId == projectId && employeeProject.EndDate == null
                               select new TeamDto
                               {
                                   EmployeeId = employeeProject.EmployeeId,
                                   UserId = employee.UserId,
                                   Username = user.Name,
                                   LeadId = _dbContext.Lead.FirstOrDefault(x => x.Id == employeeProject.LeadId).UserId,
                               }).OrderBy(x => x.Username).ToListAsync();
            return query;
        }

        public async Task<List<TeamDto>> GetTeamEmployeelist(int teamId)
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
                                   EmployeeName = user.Name
                               }
                ).OrderBy(x => x.EmployeeName).ToListAsync();
            return query;
        }

        public async Task<bool> AssignEmployeeToTeam(TeamEmployee[] teamEmployee)
        {
            var teamempList = _dbContext.TeamEmployee.Where(x => x.TeamId == teamEmployee[0].TeamId).ToList();

            foreach (var obj in teamEmployee)
            {
                TeamEmployee emp = teamempList.FirstOrDefault(x => x.EmployeeId == obj.EmployeeId && x.EndDate == null);

                if (emp == null)
                {
                    TeamEmployee teamEmployee1 = new TeamEmployee()
                    {
                        TeamId = obj.TeamId,
                        EmployeeId = obj.EmployeeId,
                        LeadId = teamempList?.FirstOrDefault()?.LeadId,
                        StartDate = DateTime.Now,
                        CreatedBy = obj.CreatedBy,
                        UpdatedBy = obj.UpdatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,

                    };
                    await _dbContext.TeamEmployee.AddAsync(teamEmployee1);
                }
            }

            foreach (var obj in teamempList)
            {
                var emp = teamEmployee.FirstOrDefault(x => x.EmployeeId == obj.EmployeeId && obj.EndDate == null);

                if (emp == null)
                {
                    if (obj.EndDate == null)
                    {
                        obj.EndDate = DateTime.Now;
                        _dbContext.TeamEmployee.Update(obj);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> AddTeam(User user, Team Team)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                Team Teams = new Team()
                {
                    Name = Team.Name,
                    StartDate = Team.StartDate,
                    EndDate = Team.EndDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = employee.UserId.ToString(),
                    UpdatedBy = employee.UserId.ToString(),
                    UpdatedDate = DateTime.Now,
                };
                var TeamDetails = await _dbContext.Team.AddAsync(Teams);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public async Task<bool> Updateteam(User user, Team team)
        {
            try
            {
                var existingTeam = await _dbContext.Team.FindAsync(team.Id);
                if (existingTeam == null)
                {
                    return false;
                }

                existingTeam.Name = team.Name;
                existingTeam.StartDate = team.StartDate;
                existingTeam.EndDate = DateTime.Now;
                existingTeam.CreatedDate = DateTime.Now;
                existingTeam.UpdatedDate = DateTime.Now;
                existingTeam.UpdatedBy = team.UpdatedBy;

                _dbContext.Entry(existingTeam).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddTeamObjective(User user, TeamObjective teamObjective)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                TeamObjective myTeamObjective = new()
                {
                    TeamId = teamObjective.TeamId,
                    Description = teamObjective.Description,
                    Status = teamObjective.Status,
                    Percentage = teamObjective.Percentage,
                    CreatedDate = DateTime.Now,
                    CreatedBy = employee.UserId.ToString(),
                    UpdatedBy = employee.UserId.ToString(),

                };

                _dbContext.TeamObjective.Add(myTeamObjective);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateTeamObjective(User user, TeamObjective updatedObjective)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var teamObjective = await _dbContext.TeamObjective.FindAsync(updatedObjective.Id);
                if (teamObjective != null)
                {
                    teamObjective.Id = updatedObjective.Id;
                    teamObjective.TeamId = teamObjective.TeamId;
                    teamObjective.Description = updatedObjective.Description;
                    teamObjective.Status = updatedObjective.Status;
                    teamObjective.Percentage = teamObjective.Percentage;
                    teamObjective.UpdatedDate = DateTime.Now;
                    teamObjective.UpdatedBy = employee.UserId.ToString();
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<TeamObjective>> GetTeamObjectiveList(int teamId)
        {
            try
            {
                var teamObjective = await _dbContext.TeamObjective.Where(x => x.TeamId == teamId).ToListAsync();
                return teamObjective;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TeamObjective> GetTeamObjectiveById(int Id)
        {
            try
            {
                var teamObjective = await _dbContext.TeamObjective.SingleOrDefaultAsync(x => x.Id == Id);
                return teamObjective;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TeamProjectDto>> GetTeamProjectList(User sessionUser,int teamId)
        {
            try
            {
                var result = (from teamProject in _dbContext.TeamProject
                              join project in _dbContext.Project on teamProject.ProjectId equals project.Id
                              where teamProject.TeamId == teamId && teamProject.EndDate == null
                              select new TeamProjectDto()
                              {
                                  id = project.Id,
                                  ProjectName = project.Name,
                                  TeamId = teamProject.TeamId,
                                  ProjectId = teamProject.ProjectId,
                                  Percentage = project.Percentage,
                                  Type = project.Type,
                                  Description = project.Description,

                              }
                          ).ToList();

                if (sessionUser.UserType == "Customer")
                {
                    var customer = _dbContext.Employee.FirstOrDefault(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == customer.Id).Select(x => x.ProjectId).ToListAsync();
                    result = result.Where(x => customerProject.Any(cp => cp == x.ProjectId)).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddTeamProject(User user, TeamProject[] teamProject)
        {
            try
            {
                var teamProjectList = _dbContext.TeamProject.Where(x => x.TeamId == teamProject[0].TeamId).ToList();

                foreach (var obj in teamProject)
                {
                    TeamProject emp = teamProjectList.FirstOrDefault(x => x.ProjectId == obj.ProjectId && x.EndDate == null);

                    if (emp == null)
                    {
                        TeamProject employeeProject1 = new TeamProject()
                        {
                            ProjectId = obj.ProjectId,
                            TeamId = obj.TeamId,
                            StartDate = DateTime.Now,
                            CreatedBy = user.Id.ToString(),
                            UpdatedBy = user.Id.ToString(),
                            CreatedDate = DateTime.Now,

                        };
                        await _dbContext.TeamProject.AddAsync(employeeProject1);
                    }
                }

                foreach (var obj in teamProjectList)
                {
                    var teaproj = teamProject.FirstOrDefault(x => x.ProjectId == obj.ProjectId && obj.EndDate == null);

                    if (teaproj == null)
                    {
                        if (obj.EndDate == null)
                        {
                            obj.EndDate = DateTime.Now;
                            _dbContext.TeamProject.Update(obj);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EmployeeTask>> GetTeamEmployeeTaskList(int Id)
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(7);
            var tasks = await _dbContext.EmployeeTask.Where(x => x.EmployeeId == Id && x.EstStartDate >= startDate && x.EstStartDate <= endDate).ToListAsync();

            return tasks;
        }


        public async Task<List<Project>> GetProjectList(int teamId, User sessionUser)
        {
            try
            {
                var result = (from Project in _dbContext.Project
                              join TeamProject in _dbContext.TeamProject on Project.Id equals TeamProject.ProjectId
                              where TeamProject.TeamId == teamId
                              select new Project()
                              {
                                  Name = Project.Name,
                                  Type = Project.Type,
                                  Description = Project.Description,
                                  Percentage = Project.Percentage,
                                  Id = Project.Id,
                                  TeamId = TeamProject.TeamId
                              }
                          ).ToList();
                if (sessionUser.UserType == "Customer")
                {
                    var employee = await _dbContext.Employee.FirstOrDefaultAsync(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == employee.Id).ToListAsync();
                    result = result.Where(x => customerProject.Any(cp => cp.ProjectId == x.Id)).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddTeamWeeklyObjective(User user, TeamWeeklyObjective teamWeeklyObjective)
        {
            try
            {

                TeamWeeklyObjective teamWeeklyObj = new TeamWeeklyObjective()
                {
                    TeamId = teamWeeklyObjective.TeamId,
                    Name = teamWeeklyObjective.Name,
                    Description = teamWeeklyObjective.Description,
                    Status = "In-Progress",
                    Percentage = 0,
                    MonthlyObjectiveId = teamWeeklyObjective.MonthlyObjectiveId,
                    ProjectId = teamWeeklyObjective.ProjectId,
                    Priority = teamWeeklyObjective.Priority,
                    CreatedBy = user.Id.ToString(),
                    UpdatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    WeekEndingDate = teamWeeklyObjective.WeekEndingDate,
                    UpdatedDate = DateTime.Now

                };
                await _dbContext.TeamWeeklyObjective.AddAsync(teamWeeklyObj);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TeamWeeklyObjective>> GetTeamWeeklyObjective(User sessionUser,int teamId)
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
                var teamweklyObjective = await _dbContext.TeamWeeklyObjective.Where(x => x.TeamId == teamId).ToListAsync();

                if (sessionUser.UserType == "Customer")
                {
                    var customer = _dbContext.Employee.FirstOrDefault(x => x.UserId == sessionUser.Id);
                    var customerProject = await _dbContext.CustomerProject.Where(x => x.EmployeeId == customer.Id).Select(x => x.ProjectId).ToListAsync();
                    teamweklyObjective = teamweklyObjective.Where(x => customerProject.Any(cp => cp == x.ProjectId)).ToList();
                }

                var projects = await _dbContext.Project.ToListAsync();

                for (int i = 0; i < teamweklyObjective.Count; i++)
                {
                    var name = projects.FirstOrDefault(x => x.Id == teamweklyObjective[i].ProjectId);
                    if (name != null)
                        teamweklyObjective[i].ProjectName = name.Name;
                }

                return teamweklyObjective;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTeamWeeklyObjective(User user, TeamWeeklyObjective teamWeeklyObjective)
        {

            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var teamObjective = await _dbContext.TeamWeeklyObjective.FirstOrDefaultAsync(x => x.Id == teamWeeklyObjective.Id);
                if (teamObjective != null)
                {
                    teamObjective.Id = teamWeeklyObjective.Id;
                    teamObjective.TeamId = teamObjective.TeamId;
                    teamObjective.Name = teamWeeklyObjective.Name;
                    teamObjective.Description = teamWeeklyObjective.Description;
                    teamObjective.Status = teamWeeklyObjective.Status;
                    teamObjective.Percentage = teamObjective.Percentage;
                    teamObjective.WeekEndingDate = teamWeeklyObjective.WeekEndingDate;
                    teamObjective.UpdatedDate = DateTime.Now;
                    teamObjective.UpdatedBy = employee.UserId.ToString();
                    _dbContext.TeamWeeklyObjective.Update(teamObjective);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> AddTeamMonthlyObjective(User user, TeamMonthlyObjective teamMonthlyObjective)
        {
            try
            {

                TeamMonthlyObjective teamMonthlyObj = new TeamMonthlyObjective()
                {
                    TeamId = teamMonthlyObjective.TeamId,
                    Name = teamMonthlyObjective.Name,
                    Description = teamMonthlyObjective.Description,
                    Status = "In-Progress",
                    Percentage = 0,
                    ProjectId = teamMonthlyObjective.ProjectId,
                    Priority = teamMonthlyObjective.Priority,
                    CreatedBy = user.Id.ToString(),
                    UpdatedBy = user.Id.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now

                };
                await _dbContext.TeamMonthlyObjective.AddAsync(teamMonthlyObj);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TeamMonthlyObjective>> GetTeamMonthlyObjective(int teamId)
        {
            try
            {
                var teamMonthlyObjective = await _dbContext.TeamMonthlyObjective.Where(x => x.TeamId == teamId).OrderByDescending(x => x.Id).ToListAsync();
                var projects = await _dbContext.Project.ToListAsync();
                var teamWeeklyObjective = await _dbContext.TeamWeeklyObjective.OrderByDescending(x => x.Id).ToListAsync();

                for (int i = 0; i < teamMonthlyObjective.Count; i++)
                {
                    var name = projects.FirstOrDefault(x => x.Id == teamMonthlyObjective[i].ProjectId).Name;
                    teamMonthlyObjective[i].ProjectName = name;
                    teamMonthlyObjective[i].TeamWeeklyObjectives = teamWeeklyObjective.Where(x => x.MonthlyObjectiveId == teamMonthlyObjective[i].Id).ToList();

                    for (int j = 0; j < teamMonthlyObjective[i].TeamWeeklyObjectives.Count; j++)
                    {
                        var projectName = projects.FirstOrDefault(x => x.Id == teamMonthlyObjective[i].ProjectId).Name;
                        teamMonthlyObjective[i].TeamWeeklyObjectives[j].ProjectName = projectName;
                    }
                }

                return teamMonthlyObjective;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> UpdataTeamMonthlyObjective(User user, TeamMonthlyObjective teamMonthlyObjective)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var teamObjective = await _dbContext.TeamMonthlyObjective.FirstOrDefaultAsync(x => x.Id == teamMonthlyObjective.Id);
                if (teamObjective != null)
                {
                    teamObjective.Name = teamMonthlyObjective.Name;
                    teamObjective.Description = teamMonthlyObjective.Description;
                    teamObjective.Status = teamMonthlyObjective.Status;
                    teamObjective.Percentage = teamObjective.Percentage;
                    teamObjective.UpdatedDate = DateTime.Now;
                    teamObjective.ProjectId = teamMonthlyObjective.ProjectId;
                    teamObjective.UpdatedBy = employee.UserId.ToString();
                    _dbContext.TeamMonthlyObjective.Update(teamObjective);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<TeamLeaveVM> GetTeamleavedetails(int TeamId, int year, int month)
        {
            try
            {
                var teamLeaveDetails = await (from teamEmployee in _dbContext.TeamEmployee
                                              join employee in _dbContext.Employee on teamEmployee.EmployeeId equals employee.Id
                                              join user in _dbContext.Users on employee.UserId equals user.Id
                                              join team in _dbContext.Team on teamEmployee.TeamId equals team.Id
                                              where teamEmployee.TeamId == TeamId && teamEmployee.EndDate == null
                                              select new TeamLeavedetailsDto
                                              {
                                                  EmployeeName = user.Name,
                                                  TeamId = team.Id,
                                                  TeamName = team.Name,
                                                  EmployeeId = employee.Id
                                              }).ToListAsync();

  

                foreach (var obj in teamLeaveDetails)
                {
                    obj.EmployeeTimes = _dbContext.EmployeeTime.Where(x => x.EmployeeId == obj.EmployeeId && x.CreatedDate.HasValue && x.CreatedDate.Value.Year == year && x.CreatedDate.Value.Month == month).ToList();
                }

                var days = _dbContext.Day.Where(x => x.Date.Month == month).ToList();
                TeamLeaveVM teamLeaveVM = new TeamLeaveVM();
                teamLeaveVM.TeamLeaveDetails = teamLeaveDetails;
                teamLeaveVM.Days = days;

                return teamLeaveVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
