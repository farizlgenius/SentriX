namespace Core.Contract.DTOs.Turnstile;

public sealed record CreateTurnstileDto(
      string Name,
      List<CreateLaneDto> Lanes,
      Guid LocationGuid
);

