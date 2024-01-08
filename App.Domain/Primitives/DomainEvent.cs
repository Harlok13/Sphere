using Mediator;

namespace App.Domain.Primitives;

public abstract record DomainEvent() : INotification
{
    public bool IsPublished { get; set; }
    public DateTimeOffset DateOccured { get; protected set; } = DateTimeOffset.UtcNow;
}