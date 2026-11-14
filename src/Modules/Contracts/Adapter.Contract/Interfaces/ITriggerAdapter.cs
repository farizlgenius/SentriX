using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface ITriggerAdapter
{
     Task AddTriggerAsync(
      string mac,
      string ip,
      short deviceId,
      short trigId,
      TriggerCommand command,
      short procId,
      short sourceType,
      short sourceNo,
      short type,
      IEnumerable<short> codes,
      short tz,
      IEnumerable<short> trigVars,
      IEnumerable<short> args,
      CancellationToken ct = default
      );

      Task DeleteTriggerAsync(
            string mac,
      string ip,
      short deviceId,
      short trigId,
      TriggerCommand command,
      short procId,
      short sourceType,
      short type,
      IEnumerable<short> codes,
      short tz,
      IEnumerable<short> trigVars,
      IEnumerable<short> args,
      CancellationToken ct = default
      );

}