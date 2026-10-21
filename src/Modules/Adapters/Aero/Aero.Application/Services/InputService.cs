using Adapter.Contract.Interfaces;

namespace Aero.Application.Services;

public sealed class InputService : IInputAdapter
{
      public Task Input(string mac, string ip, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task MonitorPoint(string mac, string ip, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}