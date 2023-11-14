using System.ComponentModel.DataAnnotations;

namespace ProjectOversight.API.Dto;

public class EmployeeLoginDto
{
    [Required(ErrorMessage = "Employee Id is required")]
    public string? EmployeeId { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Phone number is not valid.")]
    public string? PhoneNumber { get; set; }
}

public class VerifyEmployeePhoneDto
{
    [Required(ErrorMessage = "Employee Id is required")]
    public string? EmployeeId { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Phone number is not valid.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "OTP is required")]
    public int? OTP { get; set; }
}