using App.Application;
using App.Application.Identity;
using App.Infra;
using App.SignalR.HubFilters;
using App.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Scrutor;

namespace Sphere.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredPipeline(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Scan(scan => scan
            .FromAssemblies(
                typeof(IApplicationAssemblyMarker).Assembly,
                typeof(IInfrastructureAssemblyMarker).Assembly)
            .AddClasses(classes =>
                classes.Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Work")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services
            .AddInfrastructure(builder)
            .AddIdentityServices()
            .AddApplication();
        
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

    public static IServiceCollection AddMediatorWithOptions(this IServiceCollection services)
    {
        return services.AddMediator(options =>
            options.ServiceLifetime = ServiceLifetime.Transient);
    }
}