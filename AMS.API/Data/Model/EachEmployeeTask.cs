using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Data.Model
{
    public class EachEmployeeTask
    {
        public List<Project> EmployeeProjects { get; set; }
        public List<EmployeeTaskDto> EmployeeDailyTask { get; set; }

        public Decimal ToatalEstTime { get; set; }

        public Decimal ToatalActTime { get; set; }

        public int TotalProject { get; set; }   

        public int TotalTask { get; set; }

        public int TotalCompleted { get; set;}

        public int TotalReadyForUAT { get; set; }
        public int TotalInProgress { get; set; }

        public List<WeeklyListDto>? WeeklyLists { get; set; }

        public List<MonthlyLists>? MonthlyLists { get; set; }



    }
}   
             