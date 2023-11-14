using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services.Interface
{
    public interface IEmployeeTaskService
    {
        Task<List<TaskDTO>> GetProjectTasklist(int Id);
        Task<Task> GetProjectTaskById(int Id);
        Task<EmployeeTask> AssignEmployeeTask(User user, EmployeeTaskDto task);
        Task<EmployeeTask> MoveTask(User user, EmployeeTaskDto task);

        Task<List<CommentsDto>> GetComments();
        Task<bool> GetEmployeeTaskbyId(int EmployeeId, int TaskId);
        Task<Task> GetTaskDetalisById(int TaskId);
        Task<EmployeeTask> GetAssignedEmployeeTaskById(int TaskId, int projectId);
        Task<List<EmployeeTaskDto>> GetWhatsapptaskListByTaskId( int employeeId, DateTime WeekEndingDate);
        Task<bool> ReassignTask(User user, EmployeeTaskDto employeeTaskDto);
    }
}
