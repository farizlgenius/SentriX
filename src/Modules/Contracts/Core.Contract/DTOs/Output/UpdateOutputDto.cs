using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record UpdateOutputDto(
  Guid Guid,
  string Name,
  int SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);