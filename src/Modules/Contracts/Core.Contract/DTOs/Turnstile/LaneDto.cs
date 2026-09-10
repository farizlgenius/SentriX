using Core.Contract.DTOs.Door;

namespace Core.Contract.DTOs.Turnstile;

public sealed record LaneDto(
      Guid Guid,
      int LaneNo,
      List<DoorDto> Doors,
       Guid LocationGuid,
      bool IsActive,
      bool IsDefault
);