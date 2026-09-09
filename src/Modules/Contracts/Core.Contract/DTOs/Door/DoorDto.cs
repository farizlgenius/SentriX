namespace Core.Contract.DTOs.Door;

public sealed record DoorDto(
  Guid Guid,
  string Name,
  string Metadata,
  List<ReaderDto> Readers,
  BuzzerDto? Buzzer,
  RexDto? RexDto,
  SensorDto? Sensor,
  Guid LocationGuid,
  string LocationName,
  bool IsActive,
  bool IsDefault
);