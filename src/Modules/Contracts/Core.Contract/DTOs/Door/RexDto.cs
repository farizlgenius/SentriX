using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record RexDto(
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  Guid? MaskGuid
);