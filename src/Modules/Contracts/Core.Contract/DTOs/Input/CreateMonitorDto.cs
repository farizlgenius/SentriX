using SharedKernel.Enums;

namespace Core.Contract.DTOs.Monitor;

public sealed record CreateMonitorDto(
  string Name,
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  Guid LocationGuid
);