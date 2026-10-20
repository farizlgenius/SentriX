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
      
}