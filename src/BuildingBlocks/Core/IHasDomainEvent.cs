namespace Core;

public interface IHasDomainEvent
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get;}
}