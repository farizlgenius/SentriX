using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record SensorDto(
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);