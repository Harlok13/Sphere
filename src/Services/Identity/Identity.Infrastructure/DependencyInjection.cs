using System.Text;
using Identity.Application.Configurations;
using Identity.Domain.Entities;
using Identity.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddIdentityWithOptions()
            .AddAuthenticationWithOptions(configuration)
            .AddAuthorizationWithOptions();

        services.AddIdentityContext(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddIdentityWithOptions(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityContext>() 
            .AddUserManager<UserManager<User>>()
            .AddSignInManager<SignInManager<User>>();

        return services;
    }

    private static IServiceCollection AddAuthorizationWithOptions(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }

    private static IServiceCollection AddAuthenticationWithOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };

                var jwtConfiguration = configuration.Get<JwtConfiguration>();  // TODO: fix

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "Sphere",
                    ValidAudience = "Sphere",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        "SecretValidityKey@13SuperSecretKey"))
                };
            })
            .AddIdentityServerJwt();

        return services;
    }

    private static IServiceCollection AddIdentityContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("IdentityDb")));
        
        return services;
    }
}