namespace App.Domain.Primitives;

public delegate Task EventHandlerAsync<in TEntity, in TEventArgs>(
    TEntity sender,
    TEventArgs e,
    CancellationToken cT
)
    where TEntity : Entity
    where TEventArgs : class, IEventArgs;

public delegate Task EventHandlerAsync<in TEntity>(
    TEntity sender,
    CancellationToken cT
)
    where TEntity : Entity;

public delegate Task EventHandlerAsync(CancellationToken cT);

public delegate Task EventHandlerArgsAsync<in TEventArgs>(
    TEventArgs e,
    CancellationToken cT
) 
    where TEventArgs : class, IEventArgs;