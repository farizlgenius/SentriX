using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record CreateControlDto(
  string Name,
  short SlotNo,
  string Metadata,
  BaseDto Device,
  BaseDto DeviceModule
);