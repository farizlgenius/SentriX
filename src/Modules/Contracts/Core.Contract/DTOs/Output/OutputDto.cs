using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record OutputDto(
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