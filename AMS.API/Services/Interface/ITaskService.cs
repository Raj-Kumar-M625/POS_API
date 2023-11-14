using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services.Interface
{
    public interface ITaskService
    {
        Task<List<TaskListDto>> GetTaskList(TaskFilter filter, int month, int year);        
        Task<List<CustomerProject>> GetCustomerProject(int Id);
        Task<List<Task>> GetTaskListValue();
        Task<Task> CreateTask(User user, TaskDTO task);
        Task<List<UserInterface>> GetUserInterfacelist(int UserStoryId);
        Task<List<UserStory>> GetProjectUSlist(int ProjectId);
        Task<List<TaskListDto>> GetEmployeeTaskList(TaskFilter taskFilter,int employeeId);
        Task<List<CommentsDto>> GetComments(int taskId);
        Task<Task> CreateTaskCheckList(User user, TaskCheckListDto CheckList);
        Task<bool> UpdateTask(TaskDTO task,User user);
        Task<List<ProjectCheckList>> GetUserTaskCheckList(int projectId);
        Task<ProjectTask> getProjectTaskList(int projectId);
    }
}
