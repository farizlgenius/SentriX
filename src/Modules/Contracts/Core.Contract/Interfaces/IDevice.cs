using Core.Contract.DTOs.Device;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IDevice : IBase<DeviceDto, CreateDeviceDto, UpdateDeviceDto>
{
      Task<IEnumerable<TempDeviceDto>> GetScanDeviceAsync(CancellationToken ct = default);
      Task<object> GetConfigurationAsync(Guid guid,CancellationToken ct = default);
      Task<bool> ResetAsync(Guid guid,CancellationToken ct = default);
      Task<StatusDto> GetStatusAsync(Guid guid,CancellationToken ct= default);
      Task<IEnumerable<StatusDto>> GetStatusesAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
      Task<DeviceDto> GetByMacAsync(string mac,CancellationToken ct= default);
      Task<bool> GetEventStatusByGuidAsync(Guid guid,CancellationToken ct = default);
}