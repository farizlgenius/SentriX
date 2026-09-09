namespace Core.Contract.DTOs.Turnstile;

public sealed record TurnsileDto(
      Guid Guid,
      string Name,
      List<LaneDto> Lanes,
      bool IsActive,
      bool IsDefault
);