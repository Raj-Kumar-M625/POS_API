using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class Employee : BaseEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePhoto { get; set; }
        public string Department { get; set; }
        public virtual User User { get; set; }
    }
}
