using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record DoorDto(
  Guid Guid,
  string Name,
  Vendor Vendor,
  DoorType Type,
  Guid DeviceGuid,
  string DeviceName,
  string Metadata,
  List<ReaderDto> Readers,
  BuzzerDto? Buzzer,
  RexDto? RexDto,
  SensorDto? Sensor,
  RelayDto? Relay,
  BGDto? Bg,
  Guid LocationGuid,
  string LocationName,
  bool IsActive,
  bool IsDefault
);