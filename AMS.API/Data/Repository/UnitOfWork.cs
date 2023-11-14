using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectOversight.API.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProjectOversightContext _posContext;
    
    private IUserRepository? _user;
    private IEmployeeRepository? _employee;
    private IEmployeeProjectRepository? _employeeProject;
    private IProjectRepository? _project;
    private IProjectTechStackRepository? _projectTechStack;
    private ITaskRepository? _task;
    private IUploadRepository? _upload; 
    private IUserStoryUIRepository? _uiUser;
    private IEmployeeTaskRepository? _dayPlan;   
    private ICommentsRepository? _comments;
    private IUserInterfaceRepository? _userInterface;
    private IUserStoryRepository? _userStory;
    private ICategoryRepository? _category;
    private IEmployeTimeRepository? _employeeTime;
    private IEmployeeDailyTaskRepository? _timePlan;
    private ICommonMasterRepository? _commonMaster;
    private IEmployeeDailyTaskRepository? _dailyTask;
    private ILogErrorRepository? _logError;
    private IDayRepository? _day;
    private IEmployeeDayRepository? _employeeDay;
    private IProjectObjectiveRepository? _projectObjective;
    private ITeamProjectRepository _teamProject;
    private ITeamRepository? _team;
    private ISkillsetRepository? _skillset;
    private IEmployeeGeoRepository? _employeeGeo;
    private ITaskCheckListRepository? _taskCheckList;
    private IAppversionRepository _appversionRepository;
    private IUserCheckListRepository _userCheckList;
    private IWorkFlowRepository? _workFlowRepository;
    private ILeaveRepository? _leaveRepository;
    private ILeaveHistoryRepository? _leaveHistoryRepository;
    private ITaskTypeClassificationRepository? _taskTypeClassificationRepository;
    private ITeamEmployeeRepository? _teamEmployeeRepository;
    private ITeamLeadRepository? _teamLeadRepository;
    private IEmployeeWorkFlowRepository _employeeWorkFlowRepositroy;
    private readonly IDisposable _dbdispose;
    private readonly DbFactory _dbFactory;

    

    public UnitOfWork(ProjectOversightContext posContext)
    {
        _posContext = posContext;
    }
    public IUserRepository User
    {
        get { return _user ??= new UserRepository(_posContext); }
    }


    public ITeamProjectRepository TeamProject
    {
        get { return _teamProject ??= new TeamProjectRepository(_posContext); }
    }
    public IAppversionRepository Appversion
    {
        get { return _appversionRepository ??= new AppVersionRepository(_posContext); }
    }
    public IEmployeeRepository Employee
    {
        get { return _employee ??= new EmployeeRepository(_posContext); }
    }
    public IEmployeeWorkFlowRepository EmployeeWorkFlow
    {
        get { return _employeeWorkFlowRepositroy ??= new EmployeeWorkFlowRepository(_posContext); }
    }
    public IEmployeeProjectRepository EmployeeProject
    {
        get { return _employeeProject ??= new EmployeeProjectRepository(_posContext); }
    }
    public IProjectRepository Project
    {
        get { return _project ??= new ProjectRepository(_posContext); }
    }
    public IProjectTechStackRepository ProjectTechStack
    {
        get { return _projectTechStack ??= new ProjectTechStackRepository(_posContext); }
    }
    
    public ITaskRepository Task
    {
        get { return _task ??= new TaskRepository(_posContext); }
    }
    public IEmployeeTaskRepository EmployeeTask
    {
        get { return _dayPlan ??= new EmployeeTaskRepository(_posContext); }
    }
    public ITeamRepository Team
    {
        get { return _team ??= new TeamRepository(_posContext); }
    }
   
    public ICommentsRepository Comments
    {
        get { return _comments ??= new CommentsRepository(_posContext); }
    }
    public IUserInterfaceRepository UserInterface
    {
        get { return _userInterface ??= new UserInterfaceRepository(_posContext); }
    }

    public IUserStoryRepository UserStory
    {
        get { return _userStory ??= new UserStoryRepository(_posContext); }
    }
    public IUserStoryUIRepository UserStoryUI
    {
        get { return _uiUser ??= new UserStoryUIRepository(_posContext); }
    }
    public ICategoryRepository  Category
    {
        get { return _category ??= new CategoryRepository(_posContext); }
    }
    public IEmployeeDailyTaskRepository DailyTask
    {
        get { return _dailyTask ??= new EmployeeDailyTaskRepository(_posContext); }
    }
    public ICommonMasterRepository CommonMaster
    {
        get { return _commonMaster ??= new CommonMasterRepository(_posContext); }
    }

    public IEmployeTimeRepository EmployeeTime
    {
        get { return _employeeTime ??= new EmployeeTimeRepository(_posContext); }
    }
    public ILogErrorRepository LogError
    {
        get { return _logError ??= new LogErrorRepository(_posContext); }
    }
    public IProjectObjectiveRepository ProjectObjective
    {
        get { return _projectObjective ??= new ProjectObjectiveRepository(_posContext); }
    }
    
    public ISkillsetRepository skillset
    {
        get { return _skillset ??= new SkillsetRepository(_posContext); }
    }

    public IDayRepository day
    {
        get { return _day ??= new DayRepository(_posContext); }
    }
    public IEmployeeDayRepository employeeDay
    {
        get { return _employeeDay ??= new EmployeeDayRepository(_posContext); }
    }

    public IEmployeeGeoRepository EmployeeGeo
    {
        get { return _employeeGeo ??= new EmployeeGeoRepository(_posContext); }
    }

    public ITaskCheckListRepository TaskCheckList
    {
        get { return _taskCheckList ??= new TaskCheckListRepository(_posContext); }
    }

    public IUploadRepository Upload
    {
        get { return _upload ??= new UploadRepository(_posContext); }
    }
    public IUserCheckListRepository UserTaskCheckList
    {
        get { return _userCheckList ??= new UserTaskCheckListRepository(_posContext); }
    }

    public IWorkFlowRepository WorkFlow
    {
        get { return _workFlowRepository ??= new WorkFlowRepository(_posContext); }
    }

    public ITaskTypeClassificationRepository TaskTypeClassification
    {
        get { return _taskTypeClassificationRepository ??= new TaskTypeClassificationRepository(_posContext); }
    }

    public ILeaveRepository EmployeeLeave 
    {
        get { return _leaveRepository ??= new LeaveRepository(_posContext); }
    }

    public ILeaveHistoryRepository EmployeeLeaveHistory
    {
        get { return _leaveHistoryRepository ??= new LeaveHistoryRepository(_posContext); }
    }

    public ITeamEmployeeRepository TeamEmployeeRepository
    {
        get { return _teamEmployeeRepository ??= new TeamEmployeeRepository(_posContext); } 
    }

    public ITeamLeadRepository TeamLead
    {
        get { return _teamLeadRepository ??= new TeamLeadRepository(_posContext); }
    }

    public async Task<int> CommitAsync(CancellationToken token)
    {
        _dbFactory.DbContext.ChangeTracker.Entries();
        int ret = await _dbFactory.DbContext.SaveChangesAsync(token);
        _dbdispose.Dispose();
        return ret;
    }
}
