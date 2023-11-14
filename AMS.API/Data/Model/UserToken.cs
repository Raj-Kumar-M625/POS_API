using Microsoft.AspNetCore.Identity;

namespace ProjectOversight.API.Data.Model;

public class UserToken : IdentityUserToken<int>
{
    public virtual User User { get; set; }
}