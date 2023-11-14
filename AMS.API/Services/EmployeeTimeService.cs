using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using System.Linq;

namespace ProjectOversight.API.Services
{
    public class EmployeeTimeService : IEmployeeTimeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectOversightContext _dbContext;

        public EmployeeTimeService(IUnitOfWork repository, IMapper mapper, UserManager<User> userManager,
            RoleManager<Role> roleManager, IConfiguration configuration, ProjectOversightContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;

        }

        public async Task<bool> AddEmployeeTimeDetails(User user, EmployeeTimeDto LoginDetails)
        {
            try
            {
                var indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var indianTime = TimeZoneInfo.ConvertTime(DateTime.Now, indianTimeZone);
                //var Date = DateTime.Now;

                var day = await _repository.day.FindById(x => x.Date.Date == indianTime.Date);

                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                var empTime = await _repository.EmployeeTime.FindByConditionAsync(x => x.EmployeeId == employee.Id && x.DayId == day.Id);
                if(!empTime.Any())
                {
                    EmployeeDay employeeDay = new()
                    {
                        EmployeeId = employee.Id,
                        DayId = day.Id,
                        CreatedBy = employee.UserId.ToString(),
                        CreatedDate = indianTime,
                        UpdatedBy = employee.UserId.ToString(),
                    };
                    try
                    {
                        var addEmployeeDay = await _repository.employeeDay.CreateAsync(employeeDay);
                        // Handle the success case if needed
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw;
                    }
                }
                if (LoginDetails.OutTime == null)
                {
                    LoginDetails.InTime = indianTime;
                    EmployeeTime empLogin = new()
                    {
                        InTime = LoginDetails.InTime,
                        OutTime = LoginDetails.OutTime,
                        EmployeeId = employee.Id,
                        DayId = day.Id,
                        CreatedBy = employee.UserId.ToString(),
                        CreatedDate = indianTime,
                        UpdatedBy = employee.UserId.ToString(),
                    };
                    var employeeLoginDetails = await _repository.EmployeeTime.CreateAsync(empLogin);

                    var existingWork = await _repository.WorkFlow.FindByConditionAsync(x =>
                        x.WorkFlowId == 1 && x.CreatedDate.Value.Date == indianTime.Date && x.EmployeeId == employee.Id);
                    if (existingWork.Count() == 0)
                    {
                        WorkFlow workFlow = new WorkFlow
                        {
                            EmployeeId = employee.Id,
                            DayId = day.Id,
                            WorkFlowId = 1,
                            WorkedOnDate = DateTime.Now,
                            CreatedBy = user.Id.ToString(),
                            CreatedDate = DateTime.Now,
                            UpdatedBy = user.Id.ToString(),
                            UpdatedDate = DateTime.Now,
                        };
                        await _repository.WorkFlow.CreateAsync(workFlow);
                    }

                    Comments comment = new()
                    {
                        EmployeeId = employee.Id,
                        EmployeeTimeId = employeeLoginDetails.Id,
                        CreatedBy = employee.UserId.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedBy = employee.UserId.ToString(),
                        Comment = LoginDetails.Comments
                    };
                    var addComments = await _repository.Comments.CreateAsync(comment);

                    EmployeeGeo empGeo = new()
                    {
                        EmployeeId = employee.Id,
                        DayId = day.Id,
                        EmployeeTimeId = employeeLoginDetails.Id,
                        Latitude = LoginDetails.Latitude,
                        Longitude = LoginDetails.Longitude,
                        CreatedDate = indianTime,
                        CreatedBy = employee.UserId.ToString(),
                    };
                    var addEmpGeo = await _repository.EmployeeGeo.CreateAsync(empGeo);
                }
                else
                {
                    var existingEmpLogin = await _repository.EmployeeTime.FindById(x => x.EmployeeId == employee.Id && x.Id == LoginDetails.Id);
                    LoginDetails.OutTime = indianTime;
                    if (existingEmpLogin != null)
                    {
                        existingEmpLogin.OutTime = LoginDetails.OutTime;
                        existingEmpLogin.UpdatedBy = employee.UserId.ToString();
                        existingEmpLogin.UpdatedDate = DateTime.Now;

                        var updatedEmployeeLoginDetails = await _repository.EmployeeTime.UpdateAsync(existingEmpLogin);
                    }
                    Comments exComments = await _repository.Comments.FindById(x => x.EmployeeId == employee.Id && x.EmployeeTimeId == LoginDetails.Id);
                    if (exComments != null)
                    {
                        exComments.UpdatedBy = employee.UserId.ToString();
                        exComments.Comment = LoginDetails.Comments;
                        exComments.UpdatedDate = DateTime.Now;

                        var addComments = await _repository.Comments.UpdateAsync(exComments);
                    };
                    EmployeeGeo empGeo = new()
                    {
                        EmployeeId = employee.Id,
                        DayId = day.Id,
                        EmployeeTimeId = existingEmpLogin.Id,
                        Latitude = LoginDetails.Latitude,
                        Longitude = LoginDetails.Longitude,
                        CreatedDate = indianTime,
                        CreatedBy = employee.UserId.ToString(),
                    };
                    var addEmpGeo = await _repository.EmployeeGeo.CreateAsync(empGeo);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<List<EmployeeTimeDto>> GetEmployeeTimeDetails(User user)
        {
            try
            {
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                DateTime today = DateTime.Today;
                var employeeTime = await _repository.EmployeeTime.FindByConditionAsync(x => x.EmployeeId == employee.Id && x.CreatedDate.HasValue && x.CreatedDate.Value.Date == today.Date);
                var employeeLoginDetailsList = employeeTime.ToList();

                List<EmployeeTimeDto> employeeDetailsWithCommentsList = new List<EmployeeTimeDto>();

                foreach (var employeeLoginDetail in employeeLoginDetailsList)
                {
                    var comments = await _repository.Comments.FindById(c => c.EmployeeTimeId == employeeLoginDetail.Id);

                    EmployeeTimeDto employeeDetailsWithComments = new EmployeeTimeDto
                    {
                        InTime = employeeLoginDetail.InTime,
                        OutTime = employeeLoginDetail.OutTime,
                        Comments = comments.Comment,
                        Id = employeeLoginDetail.Id,
                    };

                    employeeDetailsWithCommentsList.Add(employeeDetailsWithComments);
                }

                return employeeDetailsWithCommentsList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<CommentsDto>> GetComments()
        {
            try
            {
                var commentList = (from comment in _dbContext.Comments
                                   join employee in _dbContext.Employee on comment.EmployeeId equals employee.Id
                                   join user in _dbContext.Users on employee.UserId equals user.Id
                                   where comment.EmployeeTimeId != null
                                   select new CommentsDto()
                                   {
                                       Id = comment.Id,
                                       Comment = comment.Comment,
                                       Employee = user.Name,
                                       EmployeeTimeId = comment.EmployeeTimeId
                                   }).ToList();
                return commentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WorkflowDto>> GetWorkflows(int employeeId,string date)
        {
            try
            {
                List<WorkflowDto> model = new List<WorkflowDto>();
                var day = await _repository.day.FindByConditionAsync(x => x.Date.Date.ToString() == date);
                var dayIds = day.Select(x => x.Id).ToList();
                if (day != null)
                {
                    var work = await _repository.WorkFlow.FindByConditionAsync(x => x.EmployeeId == employeeId && dayIds.Contains(x.DayId));
                    model = work.Select(x => new WorkflowDto
                    {
                        WorkflowId = x.WorkFlowId,
                    }).OrderBy(x=>x.WorkflowId).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
