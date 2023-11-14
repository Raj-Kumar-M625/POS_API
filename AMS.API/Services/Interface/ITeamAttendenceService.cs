using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface ITeamAttendenceService
    {
        Task<TeamDto> GetTeamDetailsById(int employeeId);
        Task<List<TeamAttendenceDto>> GetTeamLoginDetails(int teamId);
        Task<TeamAttendanceStatDto> GetTeamAttendenceStatistics(int teamId);
        Task<List<TeamAttendenceDto>> GetMemberAttendenceList(AttendenceFilterDto attendenceFilter);
        Task<TeamAttendanceStatDto> GetMemberAttendenceStat(int employeeId);
    }
}
