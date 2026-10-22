using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record CreateOutputDto(
  string Name,
  int SlotNo,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid
);