using Core.Contract.DTOs.DeviceModule;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IDeviceModule : IBase<DeviceModuleDto, CreateDeviceModuleDto, UpdateDeviceModuleDto>
{
  Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(Guid locationGuid, Vendor vendor, CancellationToken ct = default);
  Task UploadAsync(CancellationToken ct = default);
  Task<IEnumerable<DeviceModuleDto>> GetByDeviceAsync(Guid guid,CancellationToken ct = default);
  Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
  Task<IEnumerable<OptionDto>> GetReaderSlotAsync(Guid guid,CancellationToken ct = default);
  Task<IEnumerable<OptionDto>> GetInputSlotAsync(Guid guid,CancellationToken ct = default);
  Task<IEnumerable<OptionDto>> GetOutputSlotAsync(Guid guid,CancellationToken ct = default);
}