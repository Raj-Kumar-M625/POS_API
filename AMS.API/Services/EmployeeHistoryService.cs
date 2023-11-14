using AutoMapper;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Services
{
    public class EmployeeHistoryService : IEmployeeHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly ProjectOversightContext _dbContext;

        public EmployeeHistoryService(IUnitOfWork repository, IMapper mapper, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _repository = repository;
            _dbContext = dbContext;
        }
        public async Task<List<EmployeeDailyTask>> GetEmployeeHistory(DateTime fromDate, DateTime toDate, User user)
        {
            try
            {
                string fromDateAsString = fromDate.ToString("yyyy-MM-dd");
                string toDateAsString = toDate.ToString("yyyy-MM-dd");
                DateTime targetDatefrom = DateTime.ParseExact(fromDateAsString, "yyyy-MM-dd", null);
                DateTime targetDateto = DateTime.ParseExact(toDateAsString, "yyyy-MM-dd", null);

                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var employeeHistory = await _repository.DailyTask.FindByConditionAsync(x => x.CreatedDate >= targetDatefrom && x.CreatedDate <= targetDateto && x.EmployeeId == employee.Id);
                var result = employeeHistory.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
