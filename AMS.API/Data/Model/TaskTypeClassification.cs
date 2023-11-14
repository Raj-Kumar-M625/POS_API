using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class TaskTypeClassification :BaseEntity
    {
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public string TaskType { get; set; }
        public string Taskclassification { get; set; }

    }
}
