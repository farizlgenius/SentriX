using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IAreaAdapter
{
      Task AddAreaAsync(
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
      );

      Task DisableAreaAsync(
            string mac,
            string ip,
            short deviceId,
            short areaId,
            CancellationToken ct = default
      );

      Task EnabledAreaAsync(string mac, string ip, short deviceId, short areaId, CancellationToken ct = default);
}