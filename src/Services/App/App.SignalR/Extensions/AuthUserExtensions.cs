using System.Security.Claims;
using App.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace App.SignalR.Extensions;

public static class AuthUserExtensions
{
    public static AuthUser ToAuthUser(this ClaimsPrincipal context)
    {
        Guid userId = context.GetGuidUserId();
        var userName = context.GetUserName();
        return AuthUser.Create(userId, userName);
    }

    public static AuthUser ToAuthUser(this HubCallerContext context)
    {
        Guid userId = context.GetGuidUserId();
        var userName = context.GetUserName();
        return AuthUser.Create(userId, userName, context.ConnectionId);
    }
}