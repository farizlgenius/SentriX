using SharedKernel.Enums;

namespace Core.Contract.DTOs.Input;

public sealed record CreateInputDto(
  string Name,
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  Guid LocationGuid
);