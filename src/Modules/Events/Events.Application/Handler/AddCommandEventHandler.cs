using Device.Contract.Queries;
using Events.Application.Interfaces;
using Events.Contract.Command;
using SharedKernel.Messaging;

namespace Events.Application.Handler;

public sealed class AddCommandEventHandler(IEventRepository repo,IMessageBus bus) : ICommandHandler<AddCommandEvent>
{
      public async Task HandleAsync(AddCommandEvent command, CancellationToken ct)
      {
            var res = await bus.QueryAsync(new NameAndLocationByMacQuery(command.res.Mac));
            await repo.AddCommandEvent(res.Name,res.LocationId,command.res);
      }
}