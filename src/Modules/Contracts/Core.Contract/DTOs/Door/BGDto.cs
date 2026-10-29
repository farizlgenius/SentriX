using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record BGDto(
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  Guid? Guid = null
);