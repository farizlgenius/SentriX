using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Messaging;

public sealed class MessageBus : IMessageBus
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<MessageBus> _logger;

    public MessageBus(IServiceProvider provider,ILogger<MessageBus> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    // EVENTS
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : IEvent
    {
        try
        {
            var handlers = _provider.GetServices<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
                await handler.HandleAsync(@event, ct);

        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while publishing event of type {EventType}", typeof(TEvent).Name);
        }
        
    }

    // QUERIES
    public async Task<TResult> QueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken ct = default)
    {
        try
        {
            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _provider.GetRequiredService(handlerType);

            // ⭐ THIS WAS THE LAST BUG
            return await handler.HandleAsync((dynamic)query, ct);

        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while querying of type {QueryType}", query.GetType().Name);
            throw;
        }
        
    }

    // COMMANDS
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand
    {
        try
        {
            var handler = _provider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command, ct);
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending command of type {CommandType}", typeof(TCommand).Name);
        }
        
    }
}