namespace Core.Contract.DTOs.Door;

public sealed record CreateDoorDto(
  string Name,
  string Metadata,
  List<ReaderDto> Readers,
  BuzzerDto? Buzzer,
  RexDto? RexDto,
  SensorDto Sensor,
  Guid LocationGuid
);