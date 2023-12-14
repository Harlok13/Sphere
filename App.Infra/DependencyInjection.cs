using System.Text;
using App.Application.Identity.Extensions;
using App.Application.Identity.Repositories;
using App.Application.Repositories;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Identity.Entities;
using App.Infra.Data.Context;
using App.Infra.Extensions;
using App.Infra.Identity.Repositories;
using App.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace App.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services
            .AddScoped<IPlayerRepository, PlayerRepository>()
            .AddScoped<IRoomRepository, RoomRepository>()
            .AddScoped<IPlayerHistoryRepository, PlayerHistoryRepository>()
            .AddScoped<IPlayerStatisticRepository, PlayerStatisticRepository>()
            .AddScoped<IApplicationUserRepository, ApplicationUserRepository>()
            .AddScoped<IAppUnitOfWork, AppUnitOfWork>();

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
            .AddEntityFrameworkStores<ApplicationContext>()  // TODO: Identity context
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
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration.GetIssuer(),
                    ValidAudience = builder.Configuration.GetAudience(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration.GetSecretKey()))
                };
            });

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