using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record UpdateDoorDto(
  Guid Guid,
  string Name,
  Vendor Vendor,
  DoorType Type,
  Guid DeviceGuid,
  string Metadata,
  List<ReaderDto> Readers,
  BuzzerDto? Buzzer,
  RexDto? Rex,
  BGDto? Bg,
  SensorDto Sensor,
  RelayDto Relay,
  Guid LocationGuid
);