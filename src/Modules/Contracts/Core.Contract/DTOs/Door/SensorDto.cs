using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record SensorDto(
  Guid Guid,
  int SlotNo,
  InputMode Mode,
  string Metadata,
  Vendor Vendor
);