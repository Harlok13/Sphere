namespace App.Contracts.Responses;

public sealed record NotEnoughMoneyNotificationResponse(
    Guid NotificationId,
    string NotificationText);