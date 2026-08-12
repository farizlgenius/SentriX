namespace Core.Contract.DTOs.Device;

public sealed record DeviceDto(
  Guid Guid,
  string Name,
  string SerialNumber,
  string Mac,
  string Ip,
  int Port,
  string Firmware,
  string Vendor,
  string Metadata,
  DateTime SyncedAt,
  string ConfigurationStatus,
  Guid LocationGuid,
  bool IsActive,
  bool IsDefault
);