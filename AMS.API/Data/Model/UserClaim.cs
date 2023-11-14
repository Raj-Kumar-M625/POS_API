using Microsoft.AspNetCore.Identity;

namespace ProjectOversight.API.Data.Model;

public class UserClaim : IdentityUserClaim<int>
{
    public virtual User User { get; set; }
}