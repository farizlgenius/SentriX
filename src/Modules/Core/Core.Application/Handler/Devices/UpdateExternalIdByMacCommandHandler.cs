using Core.Application.Interfaces;
using Core.Contract.Commands.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class UpdateExternalIdByMacCommandHandler(IDeviceRepository repo) : ICommandHandler<UpdateExternalIdByMacCommand>
{
      public async Task HandleAsync(UpdateExternalIdByMacCommand command, CancellationToken ct)
      {
            await repo.UpdateExternalIdByMacAsync(command.mac,command.externalId,ct);
      }
}