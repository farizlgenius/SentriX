using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AddRelayUsedCommandHandler(IDeviceRepository repo) : ICommandHandlerWithResult<AddRelayUsedCommand,bool>
{

      public async Task<bool> HandleAsync(AddRelayUsedCommand command, CancellationToken ct)
      {
            var domain = new Relay(
                  0,
                  command.RelayNumber,
                  command.ModuleId,
                  command.LocationId,
                  true
                  );
                  
            return await repo.AddRelayAsync(domain);
      }
}
