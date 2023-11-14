using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // Change the namespace for SqlParameter
using System.Data;
using ProjectOversight.API.Services.Interface;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services
{
    public class PMOService : IPMOService
    {
        private readonly ProjectOversightContext _dbContext;
        private readonly IUnitOfWork _repository;
        public PMOService(ProjectOversightContext dbContext, IUnitOfWork repository)
        {

            _dbContext = dbContext;
            _repository = repository;
        }
        public async Task<bool> AddEmployeeScrumData()
        {
            try
            {
                _dbContext.Database.ExecuteSqlRaw("EXEC [dbo].[InsertEmployeeScrumData]");
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<PMOScrum>> GetPMOList(DateTime selectedDate)
        {
            try
            {
                var day = _dbContext.Day.FirstOrDefault(x => x.Date.Date == selectedDate.Date);
                var employees = await _dbContext.Employee.Where(x => x.IsActive == true).ToListAsync();
                var pmoList = await _dbContext.PMOScrum.Where(x => x.DayId == day.Id).ToListAsync();
                pmoList = pmoList.Where(x => employees.Any(e => e.Id == x.EmployeeId)).ToList();
                var attentionDetails = await _dbContext.PayAttention.ToListAsync();

                foreach (var pmo in pmoList)
                {
                    if (pmo.PayAttention == true)
                    {
                        pmo.payAttentions = attentionDetails.Where(x => x.PMOScrumId == pmo.Id).ToList();
                    }
                }
                return pmoList;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<EmployeeYesterdayTaskDetailsDTO> GetEmployeeYesterdayTaskDetails(DateTime selectedDate, int employeeId)
        {
            try
            {
                var dailyTaskSum = (from ed in _dbContext.EmployeeDailyTask
                                    where ed.WorkedOn.Value.Date == selectedDate.Date
                                          && ed.EmployeeId == employeeId
                                    group ed by ed.EmployeeId into g
                                    select new
                                    {
                                        TotalEstTime = g.Sum(x => x.EstTime),
                                        TotalActTime = g.Sum(x => x.ActTime)
                                    }).FirstOrDefault();

                var employeeTimeSum = (from et in _dbContext.EmployeeTime
                                       where et.CreatedDate.Value.Date == selectedDate.Date
                                             && et.EmployeeId == employeeId
                                       group et by et.EmployeeId into g
                                       select new
                                       {
                                           InTime = g.Min(x => x.InTime),
                                           OutTime = g.Max(x => x.OutTime)
                                       }).FirstOrDefault();

                var result = new EmployeeYesterdayTaskDetailsDTO
                {
                    TotalEstTime = dailyTaskSum?.TotalEstTime ?? 0,
                    TotalActTime = dailyTaskSum?.TotalActTime ?? 0,
                    InTime = employeeTimeSum?.InTime,
                    OutTime = employeeTimeSum?.OutTime
                };
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> UpdateScrum(User user, PMOScrumDto pMOScrumDto)
        {
            try
            {
                var PMOScrum = await _dbContext.PMOScrum.FirstOrDefaultAsync(x => x.Id == pMOScrumDto.Id);
                PMOScrum.ScrumStatus = pMOScrumDto.ScrumStatus;
                PMOScrum.AttendanceStatus = pMOScrumDto.AttendanceStatus;
                PMOScrum.PayAttention = pMOScrumDto.PayAttention;
                PMOScrum.ScrumTime = DateTime.Now;
                PMOScrum.UpdatedDate = DateTime.Now;
                PMOScrum.UpdatedBy = user.Id.ToString();

                if (pMOScrumDto.PayAttention == true)
                {
                    foreach (var obj in pMOScrumDto.payAttentions)
                    {
                        PayAttention payAttention = new PayAttention()
                        {
                            EmployeeId = obj.EmployeeId,
                            DayId = obj.DayId,
                            Reason = obj.Reason,
                            PMOScrumId = obj.PMOScrumId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id.ToString(),
                            UpdatedBy = user.Id.ToString(),
                            UpdatedDate = DateTime.Now,
                        };
                        await _dbContext.PayAttention.AddAsync(payAttention);
                    }
                }
                _dbContext.PMOScrum.Update(PMOScrum);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
