using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserInteraction.Infrastructure.Context;

namespace UserInteraction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUserInteractionInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddUserInteractionContext(configuration);
        
        return services;
    }

    private static IServiceCollection AddUserInteractionContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<UserInteractionContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("UserInteractionDb")));

        return services;
    }
}