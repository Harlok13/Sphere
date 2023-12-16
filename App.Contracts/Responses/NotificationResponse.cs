namespace App.Contracts.Responses;

public sealed record NotificationResponse(
    Guid Id,
    string NotificationText);