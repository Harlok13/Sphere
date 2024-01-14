namespace App.Application.Identity.Exceptions;

public enum JwtValueMsgEnum
{
    TokenValidityInMinutesError,
    RefreshTokenValidityInDaysError,
    ExpireError
}

public class JwtValueException : ApplicationException
{
    private string ErrorTimeStamp { get; set; }

    public JwtValueException(JwtValueMsgEnum errorEnumMsg) : base(GetErrorMsg(errorEnumMsg))
    {
        ErrorTimeStamp = new DateTime().ToLongTimeString();
    }

    private static string GetErrorMsg(JwtValueMsgEnum enumValue)
    {
        switch (enumValue)
        {
            case JwtValueMsgEnum.TokenValidityInMinutesError:
                return "The current value for \"Secrete_TokenValidityInMinutes\" must be greater than 0";
                
            case JwtValueMsgEnum.RefreshTokenValidityInDaysError:
                return "The current value for \"Secrete_TokenValidityInMinutes\" must be greater than 0";
            
            case JwtValueMsgEnum.ExpireError:
                return "The current value for \"Secrete_TokenValidityInMinutes\" must be greater than 0";
            
            default: return "You got this error because someone was too lazy to write tests.";
        }
    }
}