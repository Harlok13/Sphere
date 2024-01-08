namespace App.Contracts.Responses;

public sealed record NotificationResponse(
    Guid NotificationId,
    string NotificationText);