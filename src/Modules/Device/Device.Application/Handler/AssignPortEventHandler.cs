using System;
using Adapter.Abstraction.Events;
using Device.Application.Interfaces;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AssignPortEventHandler(IDeviceRepository repo) : IEventHandler<AssignPortEvent>
{
      public async Task HandleAsync(AssignPortEvent @event, CancellationToken ct)
      {
            await repo.UpdatePortByMacAsync(@event.ComponentId, @event.Port, ct);
      }
}
