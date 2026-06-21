using System;

namespace SharedKernel.Messaging;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
      Task HandleAsync(TCommand command,CancellationToken ct);
}

public interface ICommandHandlerWithResult<TCommand,TResult> where TCommand : ICommand<TResult>
{
      Task<TResult> HandleAsync(TCommand command,CancellationToken ct);
}
