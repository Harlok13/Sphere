using Infrastructure.Exceptions.DbConnectionExceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string GetApplicationContextConnectionString(
        this IConfiguration configuration,
        WebApplicationBuilder builder)
    {
        switch (builder.Environment.EnvironmentName)
        {
            case "Development":
                return configuration["ConnectionStrings:Secret_DevDb"]
                       ?? throw new DevDbConnectionStringIsNotSet("Connection string is not set.");

            case "Production":
                return configuration["ConnectionStrings:Secret_ProdDb"]
                       ?? throw new ProdDbConnectionStringIsNotSet("Connection string is not set."); 

            default: throw new InvalidEnvironmentName("Invalid environment name."); 
        }
    }
}