namespace Adapter.Contract.Interfaces;

public interface IInputAdapter
{
      Task Input(
            string mac,
            string ip,
            CancellationToken ct = default
      );
      Task MonitorPoint(
            string mac,
            string ip,
            CancellationToken ct = default
      );
}