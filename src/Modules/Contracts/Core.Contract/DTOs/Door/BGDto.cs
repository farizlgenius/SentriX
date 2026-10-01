using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record BGDto(
  int SlotNo,
  InputMode Mode,
  Vendor Vendor,
  Guid DeviceModuleGuid
);