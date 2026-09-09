namespace Core.Contract.DTOs.Turnstile;

public sealed record CreateTurnstileDto(
      string Name,
      List<LaneDto> Lanes,
      Guid LocationGuid
);

