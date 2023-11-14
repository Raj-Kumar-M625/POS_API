using Microsoft.AspNetCore.Identity;

namespace ProjectOversight.API.Data.Model;

public class RoleClaim : IdentityRoleClaim<int>
{
    public virtual Role Role { get; set; }
}