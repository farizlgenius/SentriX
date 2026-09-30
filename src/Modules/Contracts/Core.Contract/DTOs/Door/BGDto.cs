using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record BGDto(
  int SlotNo,
  Vendor Vendor,
  Guid DeviceModuleGuid
);