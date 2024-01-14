namespace App.Domain.Primitives;

public interface IHasDomainEvent
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get;}
}