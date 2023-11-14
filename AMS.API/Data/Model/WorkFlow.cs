namespace ProjectOversight.API.Data.Model
{
    public class WorkFlow : BaseEntity
    {
       public int EmployeeId { get; set; }
       public int WorkFlowId { get; set; }
       public int DayId { get; set; }
       public DateTime? WorkedOnDate { get; set; }

    }
}
