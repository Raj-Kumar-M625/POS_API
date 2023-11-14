using Microsoft.AspNetCore.Identity;

namespace ProjectOversight.API.Data.Model
{
    public class User : IdentityUser<int>
    {
        public int? OTP { get; set; }
        public DateTime? OTPValidity { get; set; }
        public string? UserCode { get; set; }
        public string? UserType { get; set; }
        public string? SecondaryPhoneNumber { get; set; }
        public string? SecondaryEmail { get; set; }
        public bool? IsActive { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
