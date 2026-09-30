using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record SensorDto(
  int SlotNo,
  InputMode Mode,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);