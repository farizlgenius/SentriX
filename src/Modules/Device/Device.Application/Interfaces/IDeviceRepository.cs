using System;
using Device.Contract.DTOs;

namespace Device.Application.Interfaces;

public interface IDeviceRepository
{
      Task<string> GetDeviceMacByComponentIdAsync(int componentId, CancellationToken ct);
      Task<(string Mac, int LocationId)> GetDeviceMacAndLocationIdByComponentIdAsync(int componentId, CancellationToken ct);
      Task UpdateComponentIdAsync(int componentId, string mac, CancellationToken ct=default);
      Task<bool> IsAnyWithMacAsync(string macAddress, CancellationToken ct=default);
      Task<DeviceDto> CreateAsync(Domain.Entities.Devices domain, CancellationToken ct = default);
      Task UpdatePortByMacAsync(string mac, int port, CancellationToken ct = default);
      Task UpdateIpByMacAsync(string mac, string ip, CancellationToken ct = default);
      Task VerifyDeviceMemoryAllocateStatusAsync(string mac, string status, CancellationToken ct = default);
}
