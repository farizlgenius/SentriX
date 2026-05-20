using System;
using Adapter.Abstraction.Events;
using Device.Application.Interfaces;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AssignIpEventHandler(IDeviceRepository repo) : IEventHandler<AssignIpEvent>
{
      public async Task HandleAsync(AssignIpEvent @event, CancellationToken ct)
      {
           await repo.UpdateIpByComponentIdAsync(@event.ComponentId, @event.IpAddress, ct);
      }
}
