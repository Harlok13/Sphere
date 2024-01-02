namespace App.Domain.DomainResults.CustomResults;

public sealed record RemovePlayerFromRoomDomainResult(
    int IncrementMoney) : DomainResult(Success: true, IsFailure: false, IsError: false);