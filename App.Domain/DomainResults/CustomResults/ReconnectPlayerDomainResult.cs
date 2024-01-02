using App.Domain.Entities.PlayerEntity;

namespace App.Domain.DomainResults.CustomResults;

public sealed record ReconnectPlayerDomainResult(
    Player Player) : DomainResult(Success: true, IsFailure: false, IsError: false);