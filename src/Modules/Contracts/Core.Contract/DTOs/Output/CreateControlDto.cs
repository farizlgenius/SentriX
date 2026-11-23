using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record CreateControlDto(
  string Name,
  short SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);