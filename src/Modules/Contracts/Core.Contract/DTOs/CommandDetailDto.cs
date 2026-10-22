using SharedKernel.Enums;

namespace Core.Contract.DTOs;

public sealed record CommandDetailDto(
      int DeviceId=-1,
      short DeviceExternalId=-1,
      string DeviceMac="",
      string DeviceIp="",
      int ModuleId=-1,
      short ModuleExternalId=-1,
      Vendor Vendor=Vendor.aero,
      int? LocationId=-1,
      int? ComponentId=-1,
      short? ComponentExternalId=-1
);