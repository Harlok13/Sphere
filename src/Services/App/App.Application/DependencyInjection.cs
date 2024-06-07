using App.Application.Services;
using App.Application.Services.Interfaces;
using App.GrpcClient.Configurations;
using App.GrpcClient.IdentityClient;
using App.GrpcClient.UserInteractionClient;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // var grpcUserInteractionConfiguration = configuration
        //     .GetSection("GrpcUserInteractionConfiguration")
        //     .Get<GrpcConfiguration>();
        
        services
            .AddScoped<ICardsDeckService, CardsDeckService>()
            .AddScoped<IUserInteractionClient, App.GrpcClient.UserInteractionClient.UserInteractionClient>()
            .AddScoped<IIdentityClient, App.GrpcClient.IdentityClient.IdentityClient>();
            // .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(IApplicationAssemblyMarker).Assembly,
            includeInternalTypes: true);
        // services
        //     .Decorate<IRoomRepository, RoomRepositoryNotifyDecorator>();
        return services;
    }

    // private static GrpcConfiguration? GetGrpcConfiguration(this IConfiguration configuration, string sectionName)
    // {
    //     var grpcConfiguration = configuration
    //         .GetSection(sectionName)
    //         .Get<GrpcConfiguration>();
    //
    //     return grpcConfiguration;
    // }
}