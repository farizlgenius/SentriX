using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IProcedureAdapter
{
     Task AddProcedureAsync(
          string mac, 
          string ip,
          short deviceId,
          short procId, 
          IEnumerable<(ActionType type,string? metadata,short? externalId,short sequence)> actions,
          CancellationToken ct = default
     );
     Task DeleteProcedureAsync(
          string mac,
          string ip,
          short deviceId,
          short procId,
          CancellationToken ct = default
     );


}