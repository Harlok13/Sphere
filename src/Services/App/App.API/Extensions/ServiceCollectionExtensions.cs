using System.Text;
using App.Application;
using App.Application.Identity;
using App.Application.Identity.Extensions;
using App.Application.Repositories;
using App.Application.Repositories.RoomRepository;
using App.Application.Repositories.UnitOfWork;
using App.GrpcClient.Configurations;
using App.SignalR.HubFilters;
using App.SignalR.Hubs;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Scrutor;

namespace App.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredPipeline(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // services.Scan(scan => scan
        //     .FromAssemblies(
        //         typeof(IApplicationAssemblyMarker).Assembly,
        //         typeof(IInfrastructureAssemblyMarker).Assembly)
        //     .AddClasses(classes =>
        //         classes.Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Work")))
        //     .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        //     .AsImplementedInterfaces()
        //     .WithScopedLifetime());

        services
            .AddScoped<IPlayerRepository, PlayerRepository>()
            .AddScoped<IPlayerInfoRepository, PlayerInfoRepository>()
            .AddScoped<IAppUnitOfWork, AppUnitOfWork>()
            .AddScoped<IFriendsRepository, FriendsRepository>()
            .AddScoped<IRoomRepository, RoomRepository>()
            .AddScoped<IPlayerHistoryRepository, PlayerHistoryRepository>();

        services
            .AddInfrastructure(builder)
            .AddIdentityServices()
            .AddApplication();
        
        return services;
    }
    
    public static IServiceCollection AddAuthenticationWithOptions(
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

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration.GetIssuer(),
                    ValidAudience = configuration.GetAudience(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration.GetSecretKey()))
                };
            })
            .AddIdentityServerJwt();

        return services;
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GrpcConfiguration>(configuration.GetSection("GrpcConfiguration"));
        // services.Configure<GrpcConfiguration>(configuration.GetSection("GrpcUserInteractionConfiguration"));
        // services.Configure<GrpcConfiguration>(configuration.GetSection("GrpcIdentityConfiguration"));
        
        return services;
    }

    public static IServiceCollection AddCorsWithOptions(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevPolicy", corsBuilder => corsBuilder
                .WithOrigins(builder.Configuration.GetCorsDevUrls())
                .AllowCredentials()
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
            );
        });

        return services;
    }

    public static IServiceCollection AddSignalRWithOptions(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddSignalR(
                options =>
                {
                    options.DisableImplicitFromServicesParameters = true;
                    if (builder.Environment.IsDevelopment())
                        options.EnableDetailedErrors = true;
                })
            .AddHubOptions<GlobalHub>(options =>
            {
                options.AddFilter<HubLoggerFilter>();
            });

        return services;
    }

    public static IServiceCollection AddStackExchangeRedisCacheWithOptions(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetRedisDsn();
            options.InstanceName = builder.Configuration.GetRedisInstanceName();
        });

        return services;
    }

    public static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.CheckConsentNeeded = context => true;
            options.ConsentCookie.IsEssential = true;
        });

        return services;
    }

    // public static IServiceCollection AddMediatorWithOptions(this IServiceCollection services)
    // {
    //     return services.AddMediator(options =>
    //         options.ServiceLifetime = ServiceLifetime.Transient);
    // }
}