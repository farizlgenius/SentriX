using Core.Contract.DTOs.Door;

namespace Core.Contract.DTOs.Turnstile;

public sealed record LaneDto(
      Guid Guid,
      int LaneNo,
      List<ReaderDto> Readers,
      SensorDto? Sensor,
      List<RelayDto> Relays,
       Guid LocationGuid,
      bool IsActive,
      bool IsDefault
);