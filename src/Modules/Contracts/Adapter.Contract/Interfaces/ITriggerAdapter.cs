namespace Adapter.Contract.Interfaces;

public interface ITriggerAdapter
{
     Task AddTriggerAsync(
      string mac, 
      string ip,
      short deviceId,
      short trigId, 
      string metadata,
      CancellationToken ct = default
      );

}