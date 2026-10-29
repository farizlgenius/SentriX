namespace Adapter.Contract.Interfaces;

public interface IInputAdapter
{
      Task MonitorPointAsync(
            string mac, 
            string ip, 
            short deviceExternalId,
            short moduleExternalId,
            short mpId,
            short slotNo,
            string metadata,
            CancellationToken ct = default);

      Task DeleteMonitorPointAsync(
           string mac, 
            string ip, 
            short deviceExternalId,
            short mpId,
            short slotNo,
            string metadata,
            CancellationToken ct = default 
      );

      Task MonitorPointStatusAsync(
            string mac,
            string ip,
            short deivceExternalId,
            short mpId,
            short count,
            CancellationToken ct= default
      );

      Task MaskMonitorPointAsync(
            string mac,
            string ip,
            short deivceExternalId,
            short mpId,
            CancellationToken ct= default
      );
      Task UnmaskMonitorPointAsync(
            string mac,
            string ip,
            short deivceExternalId,
            short mpId,
            CancellationToken ct= default
      );
}