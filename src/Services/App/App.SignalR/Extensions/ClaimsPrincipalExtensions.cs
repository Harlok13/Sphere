using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace App.SignalR.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetGuidUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ArgumentNullException.ThrowIfNull(value);
        return Guid.Parse(value);
    }
    
    public static string GetUserName(this ClaimsPrincipal user)
    {
        var userName = user.FindFirst("userName")?.Value ?? user.FindFirst(ClaimTypes.Name)?.Value;

        if (userName is null)
        {
            throw new Exception(nameof(userName));
        }
        return userName;
    }
    
    public static Guid GetGuidUserId(this HubCallerContext context)
    {
        var user = context.User;
        if (user is null)
        {
            throw new Exception(nameof(user));
        }
        return user.TryGetGuidUserId(new Exception(nameof(user)));
    }
    
    public static string GetUserName(this HubCallerContext context)
    {
        if (context.User is null)
        {
            throw new Exception(nameof(context.User));
        }
        return context.User.GetUserName();
    }
    
    public static Guid TryGetGuidUserId<TException>(this ClaimsPrincipal user, TException exception)
        where TException : Exception
    {
        var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (value is null)
            throw exception;
        return value.TryToGuidOrThrow(exception);
    }
    
    public static Guid TryToGuidOrThrow<TException>(this string id, TException exception)
        where TException : Exception
    {
        try
        {
            return Guid.Parse(id);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error in TryToGuidOrThrow");
            throw exception;
        }
    }
}
