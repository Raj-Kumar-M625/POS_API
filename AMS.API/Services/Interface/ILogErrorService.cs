using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface ILogErrorService 
    {
        Task<bool> AddErrorLogs(LogErrorDto logErrorDto, User user);
    }
}
