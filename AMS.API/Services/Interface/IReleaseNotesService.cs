using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface IReleaseNotesService
    {
        Task<List<Data.Model.Task>> GetAllReadyForUATTasklist(int projectId);
        Task<List<Data.Model.Task>> UpdateInUATTask(List<int> projectId);
        Task<bool> AddReleaseNotes(User user,ReleaseNotes releaseNotes);
        Task<ReleaseNotes> GetReleaseNotes(int projectId);
        Task<List<ReleaseNotesDto>> GetReleaseNotesHistory(int projectId);
    }
}
