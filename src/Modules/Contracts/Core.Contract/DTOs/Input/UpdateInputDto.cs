using SharedKernel.Enums;

namespace Core.Contract.DTOs.Input;

public sealed record UpdateInputDto(
  Guid Guid,
  string Name,
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  Guid LocationGuid
);