using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IProcedureAdapter
{
     Task AddProcedureAsync(string mac, string ip,short deviceId,short procId, Dictionary<ActionType,string> metadata,CancellationToken ct = default);
     Task DeleteProcedureAsync(
          string mac,
          string ip,
          short deviceId,
          short procId,
          CancellationToken ct = default
     );


}