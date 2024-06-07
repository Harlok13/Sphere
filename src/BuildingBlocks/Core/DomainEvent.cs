using Mediator;

namespace Core;

public abstract record DomainEvent() : INotification
{
    public bool IsPublished { get; set; }
    public DateTimeOffset DateOccured { get; protected set; } = DateTimeOffset.UtcNow;
}