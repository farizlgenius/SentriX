using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record OutputDto(
  Guid Guid,
  string Name,
  int SlotNo,
  string Metadata,
  Vendor Vendor,
  bool IsActive,
  bool IsDefault
);