using SharedKernel.Enums;

namespace Core.Contract.DTOs.Monitor;

public sealed record MonitorDto(
      Guid Guid,
  string Name,
  short SlotNo,
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