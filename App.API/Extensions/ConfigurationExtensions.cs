using Sphere.Exceptions.CorsExceptions;
using Sphere.Exceptions.RedisExceptions;

namespace Sphere.Extensions;

public static class ConfigurationExtensions
{
    public static string GetCorsPolicy(this IConfiguration configuration)
    {
        return configuration["CorsPolicy"]
            ?? throw new CorsPolicyException("Current CORS Policy is not set.");
    }

    public static string[] GetCorsDevUrls(this IConfiguration configuration)
    {
        return configuration.GetSection("Cors:DevUrls").Get<string[]>() 
               ?? throw new CorsHostsException("Allowed hosts for CORS is not set."); 
    }

    public static string GetRedisDsn(this IConfiguration configuration)
    {
        return configuration["Redis:Dsn"]
               ?? throw new RedisDsnException("Connection string for redis is not set.");  
    }

    public static string GetRedisInstanceName(this IConfiguration configuration)
    {
        return configuration["Redis:InstanceName"]
               ?? throw new RedisInstanceNameException("Instance name for redis is not set.");  
    }
}