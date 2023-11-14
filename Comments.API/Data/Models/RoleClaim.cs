using Microsoft.AspNetCore.Identity;

namespace Comments.API.Data.Models
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }

}