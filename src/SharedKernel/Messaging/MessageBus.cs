using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Messaging;

public sealed class MessageBus : IMessageBus
{
    private readonly IServiceProvider _provider;

    public MessageBus(IServiceProvider provider)
    {
        _provider = provider;
    }

    // EVENTS
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : IEvent
    {
        var handlers = _provider.GetServices<IEventHandler<TEvent>>();

        foreach (var handler in handlers)
            await handler.HandleAsync(@event, ct);
    }

    // QUERIES
    public async Task<TResult> QueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));

        dynamic handler = _provider.GetRequiredService(handlerType);

        // ⭐ THIS WAS THE LAST BUG
        return await handler.HandleAsync((dynamic)query, ct);
    }

    // COMMANDS
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand
    {
        var handler = _provider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, ct);
    }
}