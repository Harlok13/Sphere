using System.Text;
using App.Application.Identity.Extensions;
using App.Domain.Identity.Entities;
using App.Infra.Data.Context;
using App.Infra.Extensions;
using App.Infra.SignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddSingleton<IUserIdProvider, HubsUserIdProvider>();
        
        services
            .AddIdentityWithOptions(builder)
            .AddAuthenticationWithOptions(builder)
            .AddAuthorizationWithOptions();

        services.AddApplicationContext(builder);

        return services;
    }

    private static IServiceCollection AddIdentityWithOptions(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationContext>() // TODO: Identity context
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>();

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
        WebApplicationBuilder builder)
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

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration.GetIssuer(),
                    ValidAudience = builder.Configuration.GetAudience(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration.GetSecretKey()))
                };
            })
            .AddIdentityServerJwt();

        return services;
    }

    private static IServiceCollection AddApplicationContext(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddDbContext<ApplicationContext>(options
            => options.UseNpgsql(builder.Configuration.GetApplicationContextConnectionString(builder)));

        return services;
    }
}