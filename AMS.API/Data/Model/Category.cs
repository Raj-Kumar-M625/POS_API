using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Data.Model
{
    public class Category: BaseEntity
    {
        public string Categories { get; set; }
        public string SubCategory { get; set; }
        public bool UiApplicable { get; set; }
        public bool UserStoryApplicable { get; set; }
    }
}
