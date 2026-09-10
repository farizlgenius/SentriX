using Core.Contract.DTOs.Door;

namespace Core.Contract.DTOs.Turnstile;

public sealed record UpdateLaneDto(
      int LaneNo,
      List<UpdateDoorDto> Doors,
      Guid LocationGuid
);