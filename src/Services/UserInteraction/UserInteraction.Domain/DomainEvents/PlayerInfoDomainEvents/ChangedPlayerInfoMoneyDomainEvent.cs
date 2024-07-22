using Core;

namespace UserInteraction.Domain.DomainEvents.PlayerInfoDomainEvents;

public sealed record ChangedPlayerInfoMoneyDomainEvent(
    int Money,
    Guid PlayerId) : DomainEvent;