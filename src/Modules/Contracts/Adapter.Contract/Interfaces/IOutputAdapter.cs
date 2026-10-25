using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IOutputAdapter
{
      Task CreateControlPointAsync(
            string mac, 
            string ip, 
            short deviceId,
            short moduleId,
            short outputNo,
            short controlNo,
            string metadata,CancellationToken ct = default
      );

      Task DeleteControlPointAsync(string mac, string ip, short deviceId,short outputNo,short controlNo, CancellationToken ct = default);
      Task ControlPointCommandAsync(
            string mac,
            string ip,
            short deviceId,
            short controlId,
            OutputCommand command,
            short onTime,
            short offTime,
            short repeat,
            CancellationToken ct = default
      );
      
}