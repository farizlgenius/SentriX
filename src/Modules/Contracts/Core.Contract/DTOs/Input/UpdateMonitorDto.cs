using SharedKernel.Enums;

namespace Core.Contract.DTOs.Monitor;

public sealed record UpdateMonitorDto(
  Guid Guid,
   string Name,
  short SlotNo,
  string Metadata,
  BaseDto Device,
  BaseDto DeviceModule
);