using App.SignalR.Extensions;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace App.SignalR.HubFilters;

public class HubLoggerFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        Guid userId = invocationContext.Context.GetGuidUserId();
        Log.Logger.Information(
            "User {UserId}: Calling hub method {HubMethodName}",
            userId,
            invocationContext.HubMethodName
        );
        var arguments = invocationContext.HubMethodArguments.ToArray();
        Log.Logger.Information(
            "User {UserId}: Calling hub method {HubMethodName} with argument {Argument}",
            userId,
            invocationContext.HubMethodName,
            arguments[0]
        );
        
        try
        {
            return await next(invocationContext);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(
                ex,
                "User {UserId}: Exception calling hub method {HubMethodName}",
                userId,
                invocationContext.HubMethodName
            );

            throw;
        }
    }
}