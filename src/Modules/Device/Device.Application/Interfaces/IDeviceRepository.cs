using System;

namespace Device.Application.Interfaces;

public interface IDeviceRepository
{
      Task<string> GetDeviceMacByComponentIdAsync(int componentId, CancellationToken ct);
      Task<(string Mac, int LocationId)> GetDeviceMacAndLocationIdByComponentIdAsync(int componentId, CancellationToken ct);
      Task UpdateComponentIdAsync(int componentId, string mac, CancellationToken ct);
}
