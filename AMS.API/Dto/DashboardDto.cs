using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class DashboardDto
    {
        public List<EmployeeTimeDto> EmployeeTime { get; set; }
        public int totalProject { get; set; }
        public int onGoingProject { get; set; }
        public int completedProject { get; set; }
        public List<EmployeeTaskDto> employeeTaskDtos { get; set; }
        public List<TeamVM> TeamList { get; set; }
        public List<TeamEmployeeDto> TeamEmployees { get; set; }
        public List<TeamProjectDto> TeamProjects { get; set; }
        public List<TeamWeeklyProjectItem> TeamWeeklyProjectItem { get; set; }
    }
}
