using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace App.Infra.Extensions;

public static class ConfigurationExtensions
{
    public static string GetApplicationContextConnectionString(
        this IConfiguration configuration,
        WebApplicationBuilder builder)
    {
        switch (builder.Environment.EnvironmentName)
        {
            case "Development":
                return configuration.GetSection("ConnectionStrings:Secret_DevDb").Get<string>()
                       ?? throw new InvalidOperationException("Connection string is not set.");

            case "Production":
                return configuration.GetSection("ConnectionStrings:Secret_ProdDb").Get<string>()
                       ?? throw new InvalidOperationException("Connection string is not set."); 

            default: throw new InvalidOperationException("Invalid environment name."); // TODO: custom ex
        }
    }
}