using System;
using Adapter.Abstraction.Events;
using Device.Application.Interfaces;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class MemoryAllocateEventHandler(IDeviceRepository repo) : IEventHandler<MemoryAllocateEvent>
{
      public async Task HandleAsync(MemoryAllocateEvent @event, CancellationToken ct)
      {
            await repo.VerifyDeviceMemoryAllocateStatusAsync(@event.ComponentId, @event.Status, ct);
      }
}
