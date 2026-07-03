using Device.Application.Interfaces;
using Device.Contract.Command;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeviceSyncTimeCommandHandler(IDeviceRepository repo) : ICommandHandler<DeviceSyncTimeCommand>
{
      public async Task HandleAsync(DeviceSyncTimeCommand command, CancellationToken ct)
      {
            await repo.UpdateSyncTimeAsync(command.Mac,ct);
      }
}