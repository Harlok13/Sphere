using GameInteraction.Application.Services;
using GameInteraction.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameInteraction.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<ICardsDeckService, CardsDeckService>();
        
        return services;
    }
}