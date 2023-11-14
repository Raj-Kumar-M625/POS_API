using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using Task = ProjectOversight.API.Data.Model.Task;
using Comment = ProjectOversight.API.Data.Model.Comments;

namespace ProjectOversight.API.Services.Interface
{
    public interface ICommonService
    {
        Task<List<Category>> GetCategoriesList();
        Task<List<Task>> GetProjectTaskList(int ProjectId);
        Task<bool> AddComment(User user, Comment comment);
        Task<List<CommentsDto>> GetCommentsList(int EmployeeTaskId);
        Task<Comments> AddReplyComments(User user, CommentsDto ComDetails);
        Task<DashboardDto> GetDashboardData(User sessionUser);
        Task<TeamDashboardDto> GetTeamDashboardData(User sessionUser,int teamId);
        Task<int> UpdatePercentage();
        Task<int> UpdateOutTime();
        Task<List<TaskTypeClassification>> GetTasktypeClassificationList();

    }
}

