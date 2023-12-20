namespace App.Domain.Primitives;

public interface IHasDomainEvent
{
    ICollection<DomainEvent> DomainEvents { get; set; }
}