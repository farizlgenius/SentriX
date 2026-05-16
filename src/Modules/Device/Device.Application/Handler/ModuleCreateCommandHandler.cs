using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class ModuleCreateCommandHandler(IDeviceRepository repo) : ICommandHandler<ModuleCreateCommand>
{
      public async Task HandleAsync(ModuleCreateCommand command, CancellationToken ct=default)
      {
            var domain = new Module(
                  command.command.Name,
                  command.command.SerialNumber,
                  command.command.Fw,
                  command.command.Port,
                  command.command.Address,
                  command.command.Mac,
                  command.command.Model,
                  command.command.DeviceId
                  );
            await repo.CreateModuleAsync(domain,ct);
      }
}
