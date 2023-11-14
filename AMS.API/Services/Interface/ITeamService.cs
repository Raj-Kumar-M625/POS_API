using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface ITeamService
    {
        Task<List<Team>> GetTeamList(User sessionUser);
        Task<List<TeamEmployee>> GetTeamMemberList();

        Task<Team> GetTeamById(int id);
        Task<bool> AddTeam(User user, Team team);
        Task<bool> Updateteam(User user, Team team);

        Task<List<TeamDto>> GetTeamNames(int projectId);
        Task<List<TeamDto>> GetTeamEmployeelist(int teamId);
        Task<bool> AssignEmployeeToTeam(TeamEmployee[] teamEmployee);
       Task<bool> AddTeamObjective(User user, TeamObjective teamObjective);
        //Task<bool> UpdateTeamObjective(User user, TeamObjective updatedObjective);
        Task<List<TeamObjective>> GetTeamObjectiveList(int teamId);
        Task<TeamObjective> GetTeamObjectiveById(int Id);
        //Task<bool> UpdateTeamObjective(User user, TeamObjective updateTeamObjective, int id);
        Task<bool> UpdateTeamObjective(User user, TeamObjective updatedObjective);
        Task<List<TeamProjectDto>> GetTeamProjectList(User sessionUser,int teamId);
        Task<bool> AddTeamProject(User user, TeamProject[] teamProject);
        
        Task<List<EmployeeTask>> GetTeamEmployeeTaskList(int Id);

        Task<List<Project>> GetProjectList(int teamId, User sessionUser);
        Task<bool> AddTeamWeeklyObjective(User user, TeamWeeklyObjective teamWeeklyObjective);
        Task<List<TeamWeeklyObjective>> GetTeamWeeklyObjective(User sessionUser,int teamId);
        Task<bool> UpdateTeamWeeklyObjective(User user, TeamWeeklyObjective teamWeeklyObjective);
        Task<bool> AddTeamMonthlyObjective(User user, TeamMonthlyObjective teamMonthlyObjective);
        Task<List<TeamMonthlyObjective>> GetTeamMonthlyObjective(int teamId);
        Task<bool> UpdataTeamMonthlyObjective(User user, TeamMonthlyObjective teamMonthlyObjective);
        Task<TeamLeaveVM> GetTeamleavedetails(int TeamId, int year, int month);
    }
}
