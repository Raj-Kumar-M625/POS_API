using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services.Interface
{
    public interface IProjectService
    {
        Task<List<Project>> GetEmployeeProjectlist(User user);
        Task<List<Project>> GetAllProjectlist();
        Task<List<ProjectDto>> GetAllProjectlists(AttendenceFilterDto Tasklist);
        Task<List<UserInterface>> GetUserInterfacelist(int UserStoryId);
        Task<List<UserStory>> GetProjectUSlist(int ProjectId);        
        Task<EmployeeTask> CreateEmployeeDayPlan(User user, EmployeeTaskDto task);
        Task<List<Category>> GetCategoriesList();
        Task<List<ProjectDto>> GetProjectlist(User sessionUser);
        Task<Project> GetProjectById(int Id);
        Task<bool> AddEmployeeProject(User user, EmployeeProjectDto employeeProject);
        Task<bool> AddProject(User user, Project project);
        Task<bool> UpdateProject(User user, Project project,int Id);
        Task<ProjectObjectiveDto> GetProjectObjective(int ProjectId);
        Task<ProjectObjective> GetProjectObjectiveById(int Id);       
        Task<bool> AddProjectObjective(User user, ProjectObjective projectObjective);
        Task<bool> UpdateProjectObjective(User user, ProjectObjective updatedProjectObjective, int id);
        Task<List<UserStory>> GetUserStoryList(int projectId);
        Task<UserStory> AddUserStory(User user, UserStory UserStory);
        Task<bool> UpdateUserStory(User user, UserStory UserStory);
        Task<bool> AddUserStoryUI(User user, UserStoryUI[] userStoryUI);
        Task<List<UserStoryIdDto>> GetUserStoryUIList(int userStoryId);
        Task<UserInterface> AddUserInterface(User user,UserInterface userInterface);
        Task<bool> UpdateUserInterface(UserInterface userInterface);
        Task<bool> AssignEmployeeProject(User user, EmployeeProject[] employeeProject);
        Task<bool> AssignLead(User user,int userId, int projectId);
        Task<List<Project>> GetUnAssignedProjects(int TeamID);
        Task<List<UserStory>> GetAllUserStory(int projectId);
        //Task<ProjectObjective> FindByIdAsync(int projectId);
        Task<ProjectStatDto> GetProjectStatDetails(int ProjectId);
        Task<ProjectDashboardDto> getProjectDashBoardData();
        Task<List<ProjectCheckList>> getCheckListProject();
        Task<bool> UploadFiles(User user,Document document);
        Task<Document> DownloadFile(int id);
        Task<bool> DeleteFile(int id);
        //Task<List<ProjectCheckList>> getCheckListProject(int projectId);
        Task<ProjectCatergory> getEachProjectCategories(int projectId);
        Task<bool> AssignCustomerProject(User user, CustomerProject[] customerProject);
        Task<List<ReportWithCounts>> getCustomerProjectReport(int projectId);
        Task<ProjectReportVM> GetProjectReportDetails(int projectId);
    }
}
