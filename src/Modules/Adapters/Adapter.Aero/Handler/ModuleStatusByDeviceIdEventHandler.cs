using System;
using Adapter.Abstraction.Events;
using Adapter.Aero.Interfaces;
using AeroAdapter.Application.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Aero.Handler;

public sealed class ModuleStatusByDeviceIdEventHandler(ISioWriter writer,IScpRepository repo) : IEventHandler<ModuleStatusByDeviceIdEvent>
{
      public async Task HandleAsync(ModuleStatusByDeviceIdEvent @event, CancellationToken ct)
      {
            var ScpId = await repo.ScpIdByMacAsync(@event.Mac);
            await writer.SioStatusRequest((short)ScpId,@event.Mac,@event.ComponentIds,1);
      }
}
