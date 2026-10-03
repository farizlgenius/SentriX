using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record CreateTemplateDto
(
      string Name,
      Guid DeviceGuid,
      Guid DeviceModuleGuid,
      Template Type,
      Guid LocationGuid,
      Vendor Vendor
);