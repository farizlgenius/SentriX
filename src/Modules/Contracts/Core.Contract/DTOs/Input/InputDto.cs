using SharedKernel.Enums;

namespace Core.Contract.DTOs.Input;

public sealed record InputDto(
      Guid Guid,
  string Name,
  int SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceGuid,
  string DeviceName,
  Guid DeviceModuleGuid,
  string DeviceModuleName,
  Guid LocationGuid,
  string LocationName,
  bool IsActive,
  bool IsDefault
);