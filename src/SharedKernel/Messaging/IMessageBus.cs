using System;

namespace SharedKernel.Messaging;

public interface IMessageBus
{
      Task SendAsync<TCommand>(TCommand command,CancellationToken ct=default) where TCommand : ICommand;
      Task<TResult> QueryAsync<TResult>(IQuery<TResult> query,CancellationToken ct = default);
      Task PublishAsync<TEvent>(TEvent @event,CancellationToken ct=default) where TEvent : IEvent;
}
