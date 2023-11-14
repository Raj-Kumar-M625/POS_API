using Microsoft.AspNetCore.Identity;

namespace Comments.API.Data.Models
{
    public class UserToken : IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}