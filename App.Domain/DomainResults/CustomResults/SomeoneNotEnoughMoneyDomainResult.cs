namespace App.Domain.DomainResults.CustomResults;

public sealed record SomeoneNotEnoughMoneyDomainResult(
    string Reason,
    IEnumerable<Guid> PlayerIds,
    string NotificationForPlayers) : DomainResult(Success: false, IsFailure: true, IsError: false);