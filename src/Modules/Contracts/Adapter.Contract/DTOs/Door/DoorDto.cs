using SharedKernel.Enums;

namespace Adapter.Contract.DTOs.Door;

public sealed record DoorDto(
      string Mac,
      string Ip,
      short ScpId,
      short DoorId,
      DoorType Type,
      object Metadata,
      List<ReaderDto> Readers,
      BuzzerDto? Buzzer,
      RexDto? Rex,
      BGDto? Bg,
      SensorDto Sensor,
      RelayDto Relay
);