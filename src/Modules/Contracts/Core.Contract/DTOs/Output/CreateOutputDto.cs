using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record CreateOutputDto(
  string Name,
  int SlotNo,
  OutputMode Mode,
  string Metadata,
  Vendor Vendor,
  Guid DeviceModuleGuid,
  Guid LocationGuid
);