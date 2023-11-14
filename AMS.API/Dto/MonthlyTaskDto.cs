using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class MonthlyTaskDto
    {
        public DateTime WeekEndDate { get; set; }
        public List<MonthlyTaskList> Monthly { get; set; }

    }
}
