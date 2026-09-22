using Core.Application.Interfaces;
using Core.Contract.Commands.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class ConfigurationStatusCommandHandler(IDeviceRepository repo) : ICommandHandler<ConfigurationStatusCommand>
{
      public async Task HandleAsync(ConfigurationStatusCommand command, CancellationToken ct)
      {
            await repo.UpdateConfigurationStatusByMacAsync(command.mac,command.isSync,ct);
      }
}