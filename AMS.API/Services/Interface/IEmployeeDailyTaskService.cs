using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services.Interface
{
    public interface IEmployeeDailyTaskService
    {
        Task<List<EmployeeDailyTaskDto>> GetTimePlanList(User sessionUser);
        Task<IEnumerable<EmployeeDailyTaskDto>> GetEmployeeTask(int EmployeeId);
        Task<EmployeeDailyTask> AddEmployeeDailyTask(User user, EmployeeDailyTaskDto employeeDailyTask);
        Task<EmployeeDailyTask> AddEmployeeDayPlan(User user, EmployeeDailyTaskDto employeeDailyTask);
        Task<List<EmployeeDailyTaskDto>> GetEmployeeTimePlanList(int employeeId);
        Task<List<CommentsDto>> GetComments();
        Task<EmployeeDailyTask> GetEmployeeDailyTaskById(int employeeId, int projectId);
        Task<List<EmployeeDailyTask>> GetEmployeeDailyTask(int employeeTaskId);
        Task<List<EmployeeTaskDto>> GetCompletedWhatsapptaskListByTaskId(int employeeId, DateTime WeekEndingDate);
        Task<EmployeeDailyStatDto> GetEmployeeTaskStat(int employeeId, int employeeTaskId);
        Task<List<Task>> GetEmployeeDevChecklist(int TaskId);
        Task<List<TaskCheckList>> GetEmployeeTaskChecklist(int UserStoryUIID);
        Task<List<EmployeeDailyTask>> AutoTaskChangeStatus();
        Task<EachEmployeeTask> GetEachEmployeeDailyTaskById(int employeeId);
        Task<EachEmployeeTask> GetEmployeeMonthlyWise(int employeeId);

    }
}
