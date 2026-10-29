using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using SharedKernel.Enums;

namespace Aero.Application.Services;

public sealed class MonitorGroupService(
      IMonitorGroupRepository repo
) : IMonitorGroupAdapter
{
      public async Task CreateMonitorGroupAsync(string mac, string ip, short deviceExternalId, short monitorGroupExternalId, IEnumerable<(MonitorType type, short externalId)> Components, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteMonitorGroupAsync(string mac, string ip, short deviceExternalId, short monitorGroupExternalId, IEnumerable<(MonitorType type, short externalId)> Components, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task MonitorGroupCommandAsync(string mac, string ip, short command, short argument, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}