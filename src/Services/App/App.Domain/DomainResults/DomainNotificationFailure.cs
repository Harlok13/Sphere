namespace App.Domain.DomainResults;

public sealed record DomainNotificationFailure(
    string Reason,
    IEnumerable<Guid> PlayerIds,
    string NotificationForPlayers) : DomainResult(Success: false, IsFailure: true, IsError: false);