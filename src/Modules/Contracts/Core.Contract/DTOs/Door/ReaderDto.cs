using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record ReaderDto(
  int SlotNo,
  ReaderMode Mode,
  string Metadata,
  Vendor Vendor,
  ReaderDirection ReaderDirection,
  Guid DeviceModuleGuid
);