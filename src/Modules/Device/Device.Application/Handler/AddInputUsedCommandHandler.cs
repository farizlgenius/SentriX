using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AddInputUsedCommandHandler(IDeviceRepository repo) : ICommandHandlerWithResult<AddInputUsedCommand,bool>
{

      public async Task<bool> HandleAsync(AddInputUsedCommand command, CancellationToken ct)
      {
            var domain = new Input(
                  0,
                  command.InputNumber,
                  command.ModuleId,
                  command.LocationId,
                  true
                  );
                  
            return await repo.AddInputAsync(domain);
      }
}
