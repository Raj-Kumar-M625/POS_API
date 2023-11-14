using Microsoft.AspNetCore.Identity;

namespace ProjectOversight.API.Data.Model;

public class UserLogin : IdentityUserLogin<int>
{
    public virtual User User { get; set; }
}