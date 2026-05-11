using System;
using Device.Application.Interfaces;
using Device.Contract.Events;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class ComponentIdUpdatedEventHandler(IDeviceRepository repo) : IEventHandler<ComponentIdUpdatedEvent>
{
      public async Task HandleAsync(ComponentIdUpdatedEvent @event, CancellationToken ct)
      {
            throw new NotImplementedException();
      }
}
