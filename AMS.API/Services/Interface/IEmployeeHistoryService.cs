using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface IEmployeeHistoryService
    {
        Task<List<EmployeeDailyTask>> GetEmployeeHistory(DateTime fromDate, DateTime toDate, User user);
    }
}
