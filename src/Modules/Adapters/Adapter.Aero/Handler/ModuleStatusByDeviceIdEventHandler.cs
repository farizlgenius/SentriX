using System;
using Adapter.Abstraction.Events;
using Adapter.Aero.Interfaces;
using AeroAdapter.Application.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Aero.Handler;

public sealed class ModuleStatusByModuleIdEventHandler(ISioWriter writer,ISioRepository repo) : IEventHandler<ModuleStatusByModuleIdEvent>
{
      public async Task HandleAsync(ModuleStatusByModuleIdEvent @event, CancellationToken ct)
      {
            var sio = await repo.GetSioPanelConfigurationByModuleIdAsync(@event.ModuleId);
            await writer.SioStatusRequest((short)sio.aero.scp_id,sio.aero.mac,sio.sio_number,1);
      }
}
