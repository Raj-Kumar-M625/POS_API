using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class ProjectReportList
    {
        public int? projectId { get; set; }
        public int? UIid { get; set; }
        public string userstory { get; set; }
        public string UserInterfacelist { get; set;}
        public List<UserInterface> userInterfacelists { get; set;}
        public List<TaskDTO> TasKList { get; set; }
    }
    public class ReportWithCounts
    {
        public ProjectReportList Report { get; set; }
        public int UserInterfacelistsCount { get; set; }
        public int TasKListCount { get; set; }
    }

}
