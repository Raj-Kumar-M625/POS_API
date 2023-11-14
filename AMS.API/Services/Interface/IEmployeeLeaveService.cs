using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface IEmployeeLeaveService
    {
        Task<bool> ApplyLeaveRequest(User user, EmployeeLeaveDto leaveDto);
        Task<List<EmployeeLeave>> GetAllEmployeeLeaveList(int employeeId);
        Task<List<LeaveHistoryDto>> GetAllEmployeeLeaveById(int employeeId, int selectedId);
        Task<bool> EditLeaveRequest(User user, EmployeeLeaveDto leaveDto);
        Task<List<EmployeeLeaveDto>> GetLeaveByTeamId(int teamid);
        Task<List<LeaveHistoryDto>> GetLeaveList();
        Task<List<EmployeeLeaveDto>> GetLeaveRequestedDate(int? employeeId);
        Task<bool> UpdateLeaveRequest(User user ,EmployeeLeaveDto employeeleavedto);
        Task<List<Day>> GetHolidayList(int year ,int month);
        Task<bool> EditDay(Day day);

    }
}
