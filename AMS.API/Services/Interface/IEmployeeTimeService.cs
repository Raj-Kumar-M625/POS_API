using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface IEmployeeTimeService
    {
        Task<bool> AddEmployeeTimeDetails(User user, EmployeeTimeDto LoginDetails);
        Task<List<EmployeeTimeDto>> GetEmployeeTimeDetails(User user);
        Task<List<CommentsDto>> GetComments();
        Task<List<WorkflowDto>> GetWorkflows(int employeeId, string date);
    }
}
