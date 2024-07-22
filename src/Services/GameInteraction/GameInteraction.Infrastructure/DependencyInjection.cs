using GameInteraction.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameInteraction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        
        return services;
    }

    private static IServiceCollection AddGameInteractionContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<GameInteractionContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("GameInteractionDb")));
        
        return services;
    }
}