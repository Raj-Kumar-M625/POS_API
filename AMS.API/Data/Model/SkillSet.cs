using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Data.Model
{
    public class SkillSet : BaseEntity
    {     
        public string Category { get; set; }
        public string SubCategory1 { get; set; }
        public string? SubCategory2 { get; set; }
        public string? SubCategory3 { get; set; }

    }
}
