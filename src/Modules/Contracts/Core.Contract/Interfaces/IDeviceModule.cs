using Core.Contract.DTOs.DeviceModule;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IDeviceModule : IBase<DeviceModuleDto, CreateDeviceModuleDto, UpdateDeviceModuleDto>
{
  Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(Guid locationGuid, Vendor vendor, CancellationToken ct = default);
  Task UploadAsync(CancellationToken ct = default);
}