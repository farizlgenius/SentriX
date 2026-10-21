using SharedKernel.Enums;

namespace Core.Contract.DTOs.Input;

public sealed record UpdateInputDto(
  string Name,
  int SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceGuid,
  Guid DeviceModuleGuid,
  Guid LocationGuid
);