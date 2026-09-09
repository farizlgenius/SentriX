namespace Core.Contract.DTOs.Turnstile;

public sealed record TurnstileDto(
      Guid Guid,
      string Name,
      List<LaneDto> Lanes,
      Guid LocationGuid,
      string LocationName,
      bool IsActive,
      bool IsDefault
);