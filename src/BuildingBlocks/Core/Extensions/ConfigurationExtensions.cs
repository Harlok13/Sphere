using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddCoreJsonConfiguration(
        this IConfigurationBuilder builder,
        IHostEnvironment environment)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(
                path: $"core.{environment.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true)
            .Build();

        builder.AddConfiguration(configuration);

        return builder;
    }
}