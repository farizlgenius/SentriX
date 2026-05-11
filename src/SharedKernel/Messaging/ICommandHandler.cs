using System;

namespace SharedKernel.Messaging;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
      Task HandleAsync(TCommand command,CancellationToken ct);
}

