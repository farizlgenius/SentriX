using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AddInputUsedCommandHandler(IDeviceRepository repo) : ICommandHandler<AddInputUsedCommand>
{

      public async Task HandleAsync(AddInputUsedCommand command, CancellationToken ct)
      {
            var domain = new Input(
                  0,
                  command.InputNumber,
                  command.ModuleId,
                  command.LocationId,
                  true
                  );
                  
            await repo.AddInputAsync(domain);
      }
}
