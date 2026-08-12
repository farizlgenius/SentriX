namespace Core.Contract.DTOs.Device;

public sealed record UpdateDeviceDto(
  Guid Guid,
  string Name,
  string SerialNumber,
  string Mac,
  string Ip,
  int Port,
  string Firmware,
  string Vendor,
  string Metadata,
  Guid LocationGuid
);