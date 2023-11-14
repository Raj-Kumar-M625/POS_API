using Microsoft.AspNetCore.Identity;

namespace Comments.API.Data.Models
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}