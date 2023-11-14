using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Comments.API.Data.Models;
using Comments.API.Services.Interface;
using Comments.API.Services;
using Comments.API.Data;
using Microsoft.AspNetCore.Diagnostics;
using Comments.API.Data.Repository.Interface;
using Comments.API.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Initialize Database
builder.Services.AddDbContext<CommentsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                         throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
// Add Identity Framework
builder.Services.AddIdentity<User, Role>(options =>
{
    //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-._@+/ ";
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<CommentsContext>().AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
        ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project Oversight",
        Version = "v1",
        Description = "ProjectOverSight OpenAPI Documentation",
        TermsOfService = new Uri("https://subodhjena.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Manoj BJ",
            Email = "manojbj@epicminds.com",

        },
        License = new OpenApiLicense
        {
            Name = "LICX",
            Url = new Uri("https://example.com/license")
        }
    });
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

//Services
// Add CORS Policy
var MyAllowSpecificOrigins = "myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173",
                                             //for DEV or UAT
                                             "http://13.234.57.114:9025",
                                             //for training
                                             "http://13.234.57.114:9028",
                                             //for UAT
                                             "https://posuat.epicmindsitapi.in",
                                             "http://www.contoso.com")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                      });
});
// Repository Injections
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var dataContext = scope.ServiceProvider.GetRequiredService<CommentsContext>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
