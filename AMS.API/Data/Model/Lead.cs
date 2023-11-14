using ProjectOversight.API.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class Lead : BaseEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
