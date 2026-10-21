using SharedKernel.Enums;

namespace Core.Contract.DTOs.Input;

public sealed record CreateInputDto(
  string Name,
  int SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceGuid,
  Guid DeviceModuleGuid,
  Guid LocationGuid
);