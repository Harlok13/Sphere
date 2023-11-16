using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Sphere.DAL.Contexts;
using Sphere.DAL.Models;
using Sphere.Exceptions.JwtExceptions;

namespace Sphere.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationDbContext(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        services.AddDbContext<ApplicationDbContext>(options
            => options.UseNpgsql(GetConnectionString(builder)));
    }

    public static void AddAuthenticationWithOptions(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"]
                                  ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.IssuerError),

                    ValidAudience = builder.Configuration["Jwt:Audience"]
                                    ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.AudienceError),

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Secret_SecretKey"]
                        ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.SecretKeyError)))
                };
            });
    }

    public static void AddAuthorizationWithOptions(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });
    }

    public static void AddIdentityWithOptions(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>();
    }

    public static void AddCorsWithOptions(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevPolicy", corsBuilder => corsBuilder 
                .WithOrigins(builder.Configuration.GetSection("Cors:DevUrls").Get<string>() 
                             ?? throw new InvalidOperationException("Allowed hosts for CORS is not set."))
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
        });
    }

    private static string GetConnectionString(WebApplicationBuilder builder)
    {
        switch (builder.Environment.EnvironmentName)
        {
            case "Development":
                return builder.Configuration.GetSection("ConnectionStrings:Secret_DevDb").Get<string>()
                       ?? throw new InvalidOperationException("Connection string is not set.");

            case "Production":
                return builder.Configuration.GetSection("ConnectionStrings:Secret_ProdDb").Get<string>()
                       ?? throw new InvalidOperationException("Connection string is not set."); 

            default: throw new InvalidOperationException("Invalid environment name."); 
        }
    }
}