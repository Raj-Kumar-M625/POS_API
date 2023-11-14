using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class ProjectDocuments: BaseEntity
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public string DocName { get; set; }
        public string DocPath { get; set; }
        public string DocLink { get; set; }
     
    }
}
