using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Dto
{
    public class PMOScrumDto
    {
        public int Id { get; set; }
        public string? AttendanceStatus { get; set; }
        public bool PayAttention { get; set; }
        public string? ScrumStatus { get; set; }
        public List<PayAttention>? payAttentions { get; set; }
    }
}
