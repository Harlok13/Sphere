using System.Collections.Immutable;
using App.Application.Repositories.UnitOfWork;
using App.Domain.Primitives;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Infra.Data.UnitOfWork;

public abstract class UnitOfWorkFactory<TContext> : IUnitOfWorkFactory
    where TContext : DbContext
{
    protected readonly TContext Context;
    
    private readonly ILogger<UnitOfWorkFactory<TContext>> _logger;
    private readonly IPublisher _publisher;

    protected UnitOfWorkFactory(
        TContext context,
        ILogger<UnitOfWorkFactory<TContext>> logger,
        IPublisher publisher)
    {
        Context = context;
        _logger = logger;
        _publisher = publisher;
    }

    public virtual async ValueTask<bool> SaveChangesAsync(CancellationToken cT)
    {
        var result = await Context.SaveChangesAsync(cT) > 0;

        var events = Context.ChangeTracker.Entries<IHasDomainEvent>()
            .Select(e => e.Entity.DomainEvents)
            .SelectMany(e => e)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToImmutableArray();

        foreach (var @event in events)
        {
            @event.IsPublished = true;
            
            _logger.LogInformation(
                "{InvokedMethod} | Receive new domain event \"{EventName}\".",
                nameof(SaveChangesAsync),
                @event.GetType().Name);

            await _publisher.Publish(@event, cT);
        }

        return result;
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}