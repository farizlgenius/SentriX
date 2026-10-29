using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record RelayDto(
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);