using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Comments.API.Data.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
