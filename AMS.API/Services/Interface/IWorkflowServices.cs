using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Services.Interface
{
    public interface IWorkflowServices
    {
        Task<EmployeeWorkFlow> GetEmployeeWorkFlow(int employeeId, string Date);
        Task<EmployeeWorkFlow> CreateEmployeeWorkFlow(int employeeId, string Date, string workFlowName);
        Task<bool?> GetEmployeeTime(int employeeId, string Date);
    }
}
