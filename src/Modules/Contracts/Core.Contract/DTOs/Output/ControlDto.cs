using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record ControlDto(
  Guid Guid,
  string Name,
  short SlotNo,
  string Metadata,
  BaseDto Device,
  BaseDto DeviceModule,
  bool IsActive,
  bool IsDefault
);