using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AddRelayUsedCommandHandler(IDeviceRepository repo) : ICommandHandler<AddRelayUsedCommand>
{

      public async Task HandleAsync(AddRelayUsedCommand command, CancellationToken ct)
      {
            var domain = new Relay(
                  0,
                  command.RelayNumber,
                  command.ModuleId,
                  command.LocationId,
                  true
                  );
                  
            await repo.AddRelayAsync(domain);
      }
}
