using Core.Contract.DTOs.Door;

namespace Core.Contract.DTOs.Turnstile;

public sealed record CreateLaneDto(
      int LaneNo,
      List<CreateDoorDto> Doors,
      Guid LocationGuid
);