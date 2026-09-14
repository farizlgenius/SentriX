using SharedKernel.Enums;

namespace Core.Contract.DTOs.Device;

public sealed record TempDeviceDto(
      Guid Guid,
      int SerialNumber,
      string Mac,
      Vendor Vendor,
      DeviceModuleModel Model
);