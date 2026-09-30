using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record BuzzerDto(
  int SlotNo,
  OutputMode Mode,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);