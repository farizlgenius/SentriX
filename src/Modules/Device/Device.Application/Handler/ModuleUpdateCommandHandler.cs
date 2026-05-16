using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class ModuleUpdateCommandHandler(IDeviceRepository repo) : ICommandHandler<ModuleUpdateCommand>
{
      public async Task HandleAsync(ModuleUpdateCommand command, CancellationToken ct)
      {
            await repo.UpdateModuleAsync(command.Mac,command.Id,command.SerialNumber,command.Fw,command.Port);
      }
}
