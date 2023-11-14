using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using AutoMapper;

namespace ProjectOversight.API.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectOversight.API.Data.Model.Task, TaskDTO>().ReverseMap();
            CreateMap<EmployeeTask, EmployeeTaskDto>().ReverseMap();
            CreateMap<UserStoryUI, TaskDTO>().ReverseMap();
            CreateMap<UserStoryUI,EmployeeTaskDto>().ReverseMap();
            CreateMap<WorkFlow, WorkflowDto>().ReverseMap();
            CreateMap<EmployeeDailyTask, EmployeeDailyTaskDto>().ReverseMap().ForMember(d => d.EmployeeTask, opt => opt.MapFrom(s => s.EmployeeTask)).ReverseMap();
            CreateMap<ProjectOversight.API.Data.Model.Task, TaskCheckListDto>().ReverseMap();
            CreateMap<ProjectOversight.API.Data.Model.Upload, UploadDto>().ReverseMap();
            CreateMap<ProjectOversight.API.Data.Model.Task, ProjectTaskListDto>().ReverseMap();
            CreateMap<EmployeeLeave, EmployeeLeaveDto>().ReverseMap();
            //CreateMap<TimePlan, EmpWorkHistoryDto>().ReverseMap();
        }
    }
}
