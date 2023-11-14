using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services.Interface
{
    public interface IEmployeeService
    {
        Task<List<Team>> GetEmployeeTeamList(User user);
        Task<List<Employee>> GetEmployeeList();
        Task<EmployeeStatDto> GetEmployeeStatDetails(User user);
        Task<bool> UpdateEmployee(UserCreateDto userCreateDto);
        Task<bool> AssignSkill(EmployeeSkillSet[] empSkillSet);
        Task<List<EmployeeSkillSet>> GetEmployeeSkillById(int employeeId);
        Task<List<TeamDto>> GetEmployeeTask(User sessionUser, int teamId,DateTime? weekend,int ProjectId, DateTime? date);
        Task<List<EachTeamMonthlyTask>> GetEmployeeMonthlyTask(int teamId);
        Task<List<EmployeeProject>> GetEmployeeProject(int projectId);
        Task<AttendanceDto> GetEmployeeAttendance(User sessionUser,DateTime selectedDate);
        Task<EmployeeAttendanceVM> GetAttendanceByUserId(int userId, int month, int year);
        Task<EmployeeTaskVM> GetEmployeeTasks(User sessionUser,int employeeId);
        Task<List<TaskDTO>> GetEmployeeTasksId(AttendenceFilterDto TaskList);
        Task<Employee> UploadDocument(DocumentImageDto uploadDto);
        Task<Employee> DeleteDocument(DocumentImageDto uploadDto);
        Task <List<Task>> GetEmployeeTaskDetails(int employeeId,string status);
        Task<List<Task>> GetEmployeeTaskList(int Id, DateTime? weekend);
        Task<AttendanceDto> GetEmployeeAttendence(int Id,int? year, int? month);
        Task<List<AttendenceVm>> GetEmployeeLoginDetails(int Id, DateTime? weekend);
        Task<List<Employee>> GetCustomerList();
        Task<List<CustomerProject>> GetCustomerProject(int projectId);

    }
}
