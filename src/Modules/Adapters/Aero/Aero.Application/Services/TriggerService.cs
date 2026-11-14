

using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Core.Contract.Commands.Events;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class TriggerService(
      IMessageBus bus,
      ITriggerRepository repo
      ) : ITriggerAdapter
{
      public async Task AddTriggerAsync(
      string mac,
      string ip,
      short deviceId,
      short trigId,
      TriggerCommand command,
      short procId,
      short source,
      short sourceNo,
      short type,
      List<short> codes,
      short tz,
      List<short> trigVars,
      List<short> args,
      CancellationToken ct = default
      )
      {

            var res = repo.TriggerSpecification(
                  mac,
                  deviceId,
                  trigId,
                  command,
                  procId,
                  source,
                  sourceNo,
                  type,
                  codes,
                  tz,
                  [.. trigVars],
                  [.. args]
            );

            await bus.SendAsync(new AdapterEventCommand(res), ct);
      }

      public async Task AddTriggerAsync(string mac, string ip, short deviceId, short trigId, TriggerCommand command, short procId, short sourceType, short sourceNo, short type, IEnumerable<short> codes, short tz, IEnumerable<short> trigVars, IEnumerable<short> args, CancellationToken ct = default)
      {
            var res = repo.TriggerSpecification(
                  mac,
                  deviceId,
                  trigId,
                  command,
                  procId,
                  sourceType,
                  sourceNo,
                  type,
                  codes,
                  tz,
                  [.. trigVars],
                  [.. args]
            );

            await bus.SendAsync(new AdapterEventCommand(res), ct);
      }

      public Task DeleteTriggerAsync(string mac, string ip, short deviceId, short trigId, TriggerCommand command, short procId, short sourceType, short type, IEnumerable<short> codes, short tz, IEnumerable<short> trigVars, IEnumerable<short> args, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}