using Core;

namespace GameInteraction.Domain.DomainEvents.NotificationDomainEvent;

public sealed record BothSideNotifiedDomainEvent(
    Guid MainSideId, 
    Guid SecondSideId,
    string NotificationForMainSide,
    string NotificationForSecondSide) : DomainEvent;