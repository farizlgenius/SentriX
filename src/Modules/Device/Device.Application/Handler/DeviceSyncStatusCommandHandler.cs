using Device.Application.Interfaces;
using Device.Contract.Command;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeviceSyncStatusCommandHandler(IDeviceRepository repo) : ICommandHandler<DeviceSyncStatusCommand>
{
      public async Task HandleAsync(DeviceSyncStatusCommand command, CancellationToken ct)
      {
            await repo.SetDeviceSyncStatusAsync(command.Mac,command.Status);
      }
}