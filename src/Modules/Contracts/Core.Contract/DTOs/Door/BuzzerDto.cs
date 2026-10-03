using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record BuzzerDto(
  int SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);