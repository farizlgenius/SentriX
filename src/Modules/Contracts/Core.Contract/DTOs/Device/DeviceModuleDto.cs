using SharedKernel.Enums;

namespace Core.Contract.DTOs.Device;

public sealed record DeviceModuleDto(
  Guid Guid,
  string Name,
  string SerialNumber,
  string Mac,
  string Firmware,
  int Port,
  int Address,
  DeviceModuleModel Model
);