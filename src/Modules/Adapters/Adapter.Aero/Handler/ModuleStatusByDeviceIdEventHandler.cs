using System;
using Adapter.Abstraction.Command;
using Adapter.Abstraction.Events;
using Adapter.Aero.Interfaces;
using AeroAdapter.Application.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Aero.Handler;

public sealed class ModuleStatusByModuleIdEventHandler(IModuleCommand command) : ICommandHandler<ModuleStatusCommand>
{
      public async Task HandleAsync(ModuleStatusCommand com, CancellationToken ct)
      {
            command.SioStatusRequest(com.Mac,(short)com.DeviceCompnentId,com.ModuleComponentId,1);
      }
}
