using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record UpdateOutputDto(
  string Name,
  int SlotNo,
  OutputMode Mode,
  string Metadata,
  Vendor Vendor,
  Guid DeviceGuid,
  Guid DeviceModuleGuid,
  Guid LocationGuid
);