using Core.Contract.DTOs.Door;

namespace Core.Contract.DTOs.Turnstile;

public sealed record LaneDto(
      Guid Guid,
      List<ReaderDto> Readers,
      SensorDto? Sensor,
      RelayDto Relay,
      bool IsActive,
      bool IsDefault
);