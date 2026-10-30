using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Core.Contract.Commands.Events;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class MonitorGroupService(
      IMonitorGroupRepository repo,
      IMessageBus bus
) : IMonitorGroupAdapter
{
      public async Task CreateMonitorGroupAsync(string mac, string ip, short deviceExternalId, short monitorGroupExternalId, IEnumerable<(MonitorType type, short externalId)> Components, CancellationToken ct = default)
      {
            var res = repo.ConfigureMonitorPointGroup(
                  mac,
                  deviceExternalId,
                  monitorGroupExternalId,
                  (short)Components.Count(),
                  Components
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);

      }

      public async Task DeleteMonitorGroupAsync(string mac, string ip, short deviceExternalId, short monitorGroupExternalId, IEnumerable<(MonitorType type, short externalId)> Components, CancellationToken ct = default)
      {
            var res = repo.ConfigureMonitorPointGroup(
                  mac,
                  deviceExternalId,
                  monitorGroupExternalId,
                  0,
                  []
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task MonitorGroupCommandAsync(string mac, string ip,short deviceExternalId,short monitorGroupExternalId,short command, short argument, CancellationToken ct = default)
      {
            var res = repo.MonitorPointGroupArmDisarmCommand(
                  mac,
                  deviceExternalId,
                  monitorGroupExternalId,
                  command,
                  argument
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }


}