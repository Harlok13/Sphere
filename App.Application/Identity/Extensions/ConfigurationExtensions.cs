using App.Application.Identity.Exceptions;
using Microsoft.Extensions.Configuration;

namespace App.Application.Identity.Extensions;

public static class ConfigurationExtensions
{
    public static string GetAudience(this IConfiguration configuration)
    {
        var audience = configuration["Jwt:Audience"] 
                       ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.AudienceError);

        return audience;
    }

    public static string GetIssuer(this IConfiguration configuration)
    {
        var issuer = configuration["Jwt:Issuer"] 
                     ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.IssuerError);

        return issuer;
    }

    public static string GetSecretKey(this IConfiguration configuration)
    {
        var secretKey = configuration["Jwt:Secret_SecretKey"] 
                        ?? throw new JwtReadConfigurationException(JwtConfigMsgEnum.SecretKeyError);

        return secretKey;
    }

    public static int GetExpire(this IConfiguration configuration)
    {
        var expireSection = configuration.GetSection("Jwt:Secret_Expire") ??
                            throw new JwtReadConfigurationException(JwtConfigMsgEnum.ExpireError); 
        var expire = expireSection.Get<int>();

        if (expire < 1) throw new JwtValueException(JwtValueMsgEnum.ExpireError);

        return expire;
    }

    public static int GetTokenValidityInMinutes(this IConfiguration configuration)
    {
        var tokenValidityInMinutesSection = configuration.GetSection("Jwt:Secret_TokenValidityInMinutes")
                                            ?? throw new JwtReadConfigurationException(
                                                JwtConfigMsgEnum.TokenValidityInMinutesError); 
        var tokenValidityInMinutes = tokenValidityInMinutesSection.Get<int>();

        if (tokenValidityInMinutes < 1)
            throw new JwtValueException(JwtValueMsgEnum.TokenValidityInMinutesError);

        return tokenValidityInMinutes;
    }

    public static int GetRefreshTokenValidityInDays(this IConfiguration configuration)
    {
        var refreshTokenValidityInDaysSection = configuration.GetSection("Jwt:Secret_RefreshTokenValidityInDays")
                                                ?? throw new JwtReadConfigurationException(
                                                    JwtConfigMsgEnum.RefreshTokenValidityInDaysError
                                                ); 
        var refreshTokenValidityInDays = refreshTokenValidityInDaysSection.Get<int>();

        if (refreshTokenValidityInDays < 1) throw new JwtValueException(JwtValueMsgEnum.RefreshTokenValidityInDaysError);

        return refreshTokenValidityInDays;
    }
}