using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Core.Contract.Commands.Events;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class OutputService(
      IOutputRepository repo,
      IMessageBus bus
) : IOutputAdapter
{
      public async Task CreateControlPointAsync(
            string mac, 
            string ip, 
            short deviceId,
            short moduleId,
            short outputNo,
            short controlNo,
            string metadata,CancellationToken ct = default
            )
      {
            var meta = JsonHelper.Deserialize<OutputMetadata>(metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("OutputMetadata"));

            var res = repo.OutputPointSpecification(
                  mac,
                  deviceId,
                  moduleId,
                  outputNo,
                  meta.OfflineMode,
                  meta.DefaultMode
                  );

            await bus.SendAsync(new AdapterEventCommand(res),ct);

            res = repo.ControlPointConfiguration(
                  mac,
                  deviceId,
                  moduleId,
                  controlNo,
                  outputNo,
                  meta.DefaultPulse
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task DeleteControlPointAsync(string mac, string ip, short deviceId,short outputNo,short controlNo, CancellationToken ct = default)
      {
            var res = repo.ControlPointConfiguration(
                  mac,
                  deviceId,
                  -1,
                  controlNo,
                  outputNo,
                  1
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }
}