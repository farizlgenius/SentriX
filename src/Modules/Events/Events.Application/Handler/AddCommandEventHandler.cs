using Events.Application.Interfaces;
using Events.Contract.Command;
using SharedKernel.Messaging;

namespace Events.Application.Handler;

public sealed class AddCommandEventHandler(IEventRepository repo) : ICommandHandler<AddCommandEvent>
{
      public async Task HandleAsync(AddCommandEvent command, CancellationToken ct)
      {
            await repo.AddCommandEvent(command.res);
      }
}