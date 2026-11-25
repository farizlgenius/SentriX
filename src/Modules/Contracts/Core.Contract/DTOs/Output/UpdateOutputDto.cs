using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record UpdateOutputDto(
  Guid Guid,
  string Name,
  short SlotNo,
  string Metadata,
  BaseDto Device,
  BaseDto DeviceModule
);