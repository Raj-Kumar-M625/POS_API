using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class LogError
    {
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public string LogDescription { get; set; }
        public string TableName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
