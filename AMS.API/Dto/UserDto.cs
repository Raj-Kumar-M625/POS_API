using System.ComponentModel.DataAnnotations;

namespace ProjectOversight.API.Dto;

public class UserLoginDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string? VersionCode { get; set; }
}

public class UserCreateDto
{
    public int id { get; set; }
    [MaxLength(10)]
    [Display(Name = "Phone")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
    public string? PhoneNumber { get; set; }
    [MaxLength(10)]
    [Display(Name = "Phone")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
    public string? SecondaryPhoneNumber { get; set; }
    [MaxLength(20)]
    public string? UserName { get; set; }
    [MaxLength(10)]

    public string? EmployeeCode { get; set; }
    [MaxLength(100)]
    [Display(Name = "Email")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
     ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }
    [MaxLength(100)]
    [Display(Name = "Email")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
     ErrorMessage = "Invalid email format")]
    public string? SecondaryEmail { get; set; }

    //[Required(ErrorMessage = "Password is required")]
    [StringLength(20, ErrorMessage = "Must be between 8 and 20 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }


    [StringLength(20, ErrorMessage = "Must be between 8 and 20 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Compare("Password")]

    public string? ConfirmPassword { get; set; }
    [Required]

    public string? Role { get; set; }
    public string? Department { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; }

}

public class VerifyPhoneDto
{
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Phone number is not valid.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "OTP is required")]
    public int? OTP { get; set; }
}

public class PasswordUpdateDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string? ConfirmPassword { get; set; }
}

