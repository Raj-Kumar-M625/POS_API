using Microsoft.AspNetCore.Identity;

namespace Comments.API.Data.Models
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }
}