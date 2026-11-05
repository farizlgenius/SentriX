using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record InteriorPushButtonOutputDto(
  short SlotNo,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  string Metadata,
  Guid? Guid = null
);