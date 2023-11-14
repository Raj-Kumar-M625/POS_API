using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Services.Interface;
using ProjectOversight.API.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProjectOversight.API.Services
{
    public class WorkFlowServices : IWorkflowServices
    {

        private readonly IUnitOfWork _repository;
        private readonly ProjectOversightContext _dbcontext;
        public WorkFlowServices(IUnitOfWork repository,ProjectOversightContext dbcontext)
        {
          
            _repository = repository;
            _dbcontext = dbcontext;
        }
        public async Task<EmployeeWorkFlow> GetEmployeeWorkFlow(int employeeId,string Date)
        {
            try
            {
                //var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var day = await _repository.day.FindById(x => x.Date.Date.ToString() == Date);
                var employeeWorkFlow = await _repository.EmployeeWorkFlow.FindByConditionAsync(x => x.EmployeeId == employeeId && x.DayId == day.Id);
                var employee = employeeWorkFlow.OrderBy(x => x.WorkFlowId).LastOrDefault();
                return employee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<EmployeeWorkFlow> CreateEmployeeWorkFlow(int employeeId, string Date,string workFlowName)
        {
            try
            {
                var workFLow =  _dbcontext.WorkFlowType.FirstOrDefault(x => x.Name == workFlowName);
                var day = await _repository.day.FindById(x => x.Date.Date.ToString() == Date);
                var employeeWorkFlow = await _repository.EmployeeWorkFlow.FindByConditionAsync(x => x.EmployeeId == employeeId && x.DayId == day.Id 
                && x.WorkFlowId == workFLow!.Id);
                
                if (employeeWorkFlow.Count() == 0)
                {
                    var employeeFlow = new EmployeeWorkFlow()
                    {
                        EmployeeId = employeeId,
                        DayId = day.Id,
                        WorkFlowId = workFLow!.Id,
                    };
                    var employee = await _repository.EmployeeWorkFlow.CreateAsync(employeeFlow);
                    return employee;
                }
                else
                {
                    return employeeWorkFlow.FirstOrDefault()!;
                }
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool?> GetEmployeeTime(int employeeId, string Date)
        {
            try
            {
              
                var day = await _repository.day.FindById(x => x.Date.Date.ToString() == Date);
                var employeeTime = await _repository.EmployeeTime.FindByConditionAsync(x => x.EmployeeId == employeeId && x.DayId == day.Id
                );
                if(employeeTime.Count() == 0) {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
