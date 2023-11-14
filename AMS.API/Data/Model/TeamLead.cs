namespace ProjectOversight.API.Data.Model
{
    public class TeamLead : BaseEntity
    {
        public int TeamId { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
