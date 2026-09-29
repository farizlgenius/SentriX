using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record BGDto(
  Guid Guid,
  int SlotNo,
  Vendor Vendor,
  Guid DeviceModuleGuid
);