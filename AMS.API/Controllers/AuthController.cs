using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using ProjectOversight.API.Helpers.SendMail;

namespace ProjectOversight.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _repository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    public AuthController(IUnitOfWork repository, UserManager<User> userManager,
        IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager)
    {
        _repository = repository;
        _userManager = userManager;
        _mapper = mapper;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserCreateDto createUser)
    {
        var user = await _repository.User.FindByEmailAsync(createUser.Email);
        if(user != null)
        {
            return StatusCode(StatusCodes.Status409Conflict, "User already exists!");
        }

        try
        {
            // var user = _mapper.Map<User>(userLogin);
            User newUser = new()
            {
                PhoneNumber = createUser.PhoneNumber,
                UserName = createUser.UserName.Replace(" ","-") + "_" + createUser.PhoneNumber,
                NormalizedUserName = createUser.UserName.Replace(" ", "-") + "_" + createUser.PhoneNumber,
                Email = createUser.Email,
                UserType = createUser.Role,
                NormalizedEmail = createUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                SecondaryEmail = createUser.SecondaryEmail,
                SecondaryPhoneNumber = createUser.SecondaryPhoneNumber,
                IsActive = true,
                LockoutEnabled = false,
                Name = createUser.UserName 
            };
            var result = await _userManager.CreateAsync(newUser, createUser.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, createUser.Role);
                Employee newEmployee = new()
                {
                    UserId = newUser.Id,
                    Name = newUser.UserName,
                    PhoneNumber = newUser.PhoneNumber,
                    IsActive = true,
                    Department = createUser.Department,
                    Category = createUser.Category,
                    ProfilePhoto = "Sample",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedBy = newUser.Id.ToString(),
                    UpdatedBy = newUser.Id.ToString()
                };
                var employee = await _repository.Employee.CreateAsync(newEmployee);
                return Ok("User Created");
            }
            return StatusCode(StatusCodes.Status304NotModified, "Failed");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status304NotModified, "Failed ");
        }
    }
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Employeelogin(UserLoginDto userModel)
    {
        try
        {
            var VersionCode = await _repository.Appversion.GetAppVersion(userModel.VersionCode!);
            if (VersionCode == true)
            {
                return StatusCode(StatusCodes.Status404NotFound, "App Version Not Supported");
            }
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Password or Email did not Match!");
            }
            var user = await _repository.User.FindByEmailAsync(userModel.Email);
            if (user != null &&
            await _userManager.CheckPasswordAsync(user, userModel.Password))
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(identity));
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                var token = GetToken(authClaims);
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                //var apiKey = "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";
                //var mailHandler = new MailHandler(apiKey);
                //var isSuccess = await mailHandler.SendMail("manojbj46@gmail.com", "Hello, this is the email message.", "Subject of the email");
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userRoles = userRoles.FirstOrDefault(),
                    employeeId = employee.Id,
                    empPhoto = employee.ProfilePhoto,
                    userName = user.Name
                });
            }

            else
            {
                return StatusCode(StatusCodes.Status409Conflict, "Password or Email did not Match!");
            }
        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status404NotFound, "User Does Not Exist");
        }
    }

    [HttpPost("Teamlogin")]
    [AllowAnonymous]
    public async Task<IActionResult> Teamlogin(UserLoginDto userModel)
    {
        try
        {
            var VersionCode = await _repository.Appversion.GetAppVersion(userModel.VersionCode!);
            if (VersionCode == true)
            {
                return StatusCode(StatusCodes.Status404NotFound, "App Version Not Supported");
            }
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Password or Email did not Match!");
            }
            var user = await _repository.User.FindByEmailAsync(userModel.Email);
            if (user != null &&
            await _userManager.CheckPasswordAsync(user, userModel.Password))
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(identity));
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                var token = GetToken(authClaims);
                var employee = await _repository.Employee.FindById(x => x.UserId == user.Id);
                //var apiKey = "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";
                //var mailHandler = new MailHandler(apiKey);
                //var isSuccess = await mailHandler.SendMail("manojbj46@gmail.com", "Hello, this is the email message.", "Subject of the email");
                if (employee != null)
                {
                    var employeeInTeamLead = await _repository.TeamLead.FindByConditionAsync(x => x.EmployeeId == employee.Id && x.IsActive == true);

                    if (employeeInTeamLead.Any())
                    {
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            userRoles = "TeamLead",
                            employeeId = employee.Id,
                            empPhoto = employee.ProfilePhoto,
                            userName = user.Name
                        });
                    }
                }
                return StatusCode(StatusCodes.Status409Conflict, "User is Not TeamLead");
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict, "Password or Email did not Match!");
            }
        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status404NotFound, "User Does Not Exist");
        }
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

        var token = new JwtSecurityToken(
            _configuration["JwtSettings:ValidIssuer"],
            _configuration["JwtSettings:ValidAudience"],
            expires: DateTime.Now.AddDays(365),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logged Out");
    }


    [HttpPost("ChangePW")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateDto createUser)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await _repository.User.FindByEmailAsync(createUser.Email);
                if(user == null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Incorrect EmailID");
                }

                if (user != null)
                {
                    user.Email = createUser.Email;
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, createUser.ConfirmPassword);
                    //user.Email = user.Email;
                }
                var createResult = await _userManager.UpdateAsync(user);
            }
            return Ok("Password Changed Successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status304NotModified, "Failed ");
        }
    }

    

}