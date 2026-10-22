using SharedKernel.Enums;

namespace Core.Contract.DTOs;

public sealed record CommandDetailDto(
      int DeviceId,
      short DeviceExternalId,
      string DeviceMac,
      string DeviceIp,
      int ModuleId,
      short ModuleExternalId,
      Vendor Vendor,
      int? LocationId,
      int? ComponentId,
      short? ComponentExternalId
);