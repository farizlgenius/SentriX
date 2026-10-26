using Adapter.Contract.Interfaces;
using Aero.Application.Enums;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Area;
using Core.Contract.Commands.Events;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class AreaService(
      IAreaRepository repo,
      IMessageBus bus
      ) : IAreaAdapter
{
      public async Task AddAreaAsync(
            string mac,
            string ip,
            short deviceId,
            short areaId,
            AreaMultiOccMode multi,
            AreaMode mode,
            short occMax,
            short occDown,
            short occUp,
            string metadata,
            CancellationToken ct = default
      )
      {

             var meta = JsonHelper.Deserialize<AreaMetadata>(metadata);
            if (meta == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("AreaMetadata"));

            short areaflag = 0;

            if(meta.AirLock)
                  areaflag |= (short)AreaFlags.AREA_F_AIRLOCK;

            if(meta.OnlyOneDoor)
                  areaflag |= (short)AreaFlags.AREA_F_AIRLOCK_ODO;
                  
           var res = repo.ConfigureAccessArea(
            mac,
            deviceId,
            areaId,
            (short)multi,
            (short)AreaCommand.Enabled,
            0,
            0,
            occMax,
            occUp,
            occDown,
            areaflag
           );

           await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task DisableAreaAsync(string mac, string ip, short deviceId, short areaId, CancellationToken ct = default)
      {
            var res = repo.ModifyAccessAreaConfiguration(
                  mac,
                  deviceId,
                  areaId,
                  (short)AreaCommand.Disabled,
                  0
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task EnabledAreaAsync(string mac, string ip, short deviceId, short areaId, CancellationToken ct = default)
      {
            var res = repo.ModifyAccessAreaConfiguration(
                  mac,
                  deviceId,
                  areaId,
                  (short)AreaCommand.Enabled,
                  0
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }
}