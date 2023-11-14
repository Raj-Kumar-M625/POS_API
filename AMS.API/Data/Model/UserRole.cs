using Microsoft.AspNetCore.Identity;
using System.Data;

namespace ProjectOversight.API.Data.Model
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
