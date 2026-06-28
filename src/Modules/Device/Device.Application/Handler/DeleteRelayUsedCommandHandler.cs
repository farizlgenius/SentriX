using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeleteRelayUsedCommandHandler(IDeviceRepository repo) : ICommandHandler<DeleteRelayUsedCommand>
{

      public async Task HandleAsync(DeleteRelayUsedCommand command, CancellationToken ct)
      {
            var domain = new Relay(
                  0,
                  command.RelayNumber,
                  command.ModuleId,
                  0,
                  true
            );
            await repo.DeleteRelayAsync(domain);
      }
}
