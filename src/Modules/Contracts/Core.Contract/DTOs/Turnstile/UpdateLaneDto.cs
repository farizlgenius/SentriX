using Core.Contract.DTOs.Door;

namespace Core.Contract.DTOs.Turnstile;

public sealed record UpdateLaneDto(
      Guid Guid,
      int LaneNo,
      List<ReaderDto> Readers,
      SensorDto Sensor,
      Guid LocationGuid
);