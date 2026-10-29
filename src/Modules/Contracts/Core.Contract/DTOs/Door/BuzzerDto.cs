using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record BuzzerDto(
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  Guid? Guid = null
);