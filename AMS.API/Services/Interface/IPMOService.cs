using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface IPMOService
    {
        Task<bool> AddEmployeeScrumData();
        Task<List<PMOScrum>> GetPMOList(DateTime selectedDate);
        Task<EmployeeYesterdayTaskDetailsDTO> GetEmployeeYesterdayTaskDetails(DateTime selectedDate, int employeeId);
        Task<bool> UpdateScrum(User user, PMOScrumDto pMOScrumDto);
    }
}
