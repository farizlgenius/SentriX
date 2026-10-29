using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IMonitorGroupAdapter
{
      Task CreateMonitorGroupAsync(
            string mac,
            string ip,
            short deviceExternalId,
            short monitorGroupExternalId,
            IEnumerable<(MonitorType type,short externalId)> Components,
            CancellationToken ct = default
      );

      Task DeleteMonitorGroupAsync(
            string mac,
            string ip,
            short deviceExternalId,
            short monitorGroupExternalId,
            IEnumerable<(MonitorType type,short externalId)> Components,
            CancellationToken ct = default
      );

      Task MonitorGroupCommandAsync(
            string mac,
            string ip,
            short command,
            short argument,
            CancellationToken ct = default
      );
}