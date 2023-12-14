namespace App.Application.Identity.Exceptions;

public enum JwtConfigMsgEnum
{
    AudienceError,
    IssuerError,
    SecretKeyError,
    ExpireError,
    TokenValidityInMinutesError,
    RefreshTokenValidityInDaysError
}

public class JwtReadConfigurationException : ApplicationException
{
    public string ErrorTimeStamp { get; set; }

    public JwtReadConfigurationException(JwtConfigMsgEnum errorEnumMsg) : base(GetErrorMsg(errorEnumMsg))
    {
        ErrorTimeStamp = new DateTime().ToLongTimeString();
    }

    private static string GetErrorMsg(JwtConfigMsgEnum enumValue)
    {
        switch (enumValue)
        {
            case JwtConfigMsgEnum.AudienceError:
                return "An error occurred while reading a value \"Audience\" from the appsettings.";
            
            case JwtConfigMsgEnum.IssuerError:
                return "An error occurred while reading a value \"Issuer\" from the appsettings.";
            
            case JwtConfigMsgEnum.SecretKeyError:
                return "An error occurred while reading a value \"Secret_SecretKey\" from the appsettings.";
            
            case JwtConfigMsgEnum.ExpireError:
                return "An error occurred while reading a value \"Secret_Expire\" from the appsettings.";
            
            case JwtConfigMsgEnum.TokenValidityInMinutesError:
                return "An error occurred while reading a value \"Secret_TokenValidityInMinutes\" from the appsettings.";
            
            case JwtConfigMsgEnum.RefreshTokenValidityInDaysError:
                return "An error occurred while reading a value \"Secret_RefreshTokenValidityInDays\" from the appsettings.";
            
            default: return "An error occurred because I'm stupid.";
        }
    }
}