using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record RexDto(
  Guid Guid,
  int SlotNo,
  InputMode Mode,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);