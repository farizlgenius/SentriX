using SharedKernel.Enums;

namespace Core.Contract.DTOs.Device;

public sealed record CreateDeviceDto(
  string Name,
  string SerialNumber,
  string Mac,
  string Ip,
  int Port,
  string Firmware,
  Vendor Vendor,
  string Metadata,
  Guid LocationGuid
);