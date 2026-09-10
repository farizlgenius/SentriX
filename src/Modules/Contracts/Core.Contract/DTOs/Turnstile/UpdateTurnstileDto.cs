namespace Core.Contract.DTOs.Turnstile;

public sealed record UpdateTurnstileDto(
      Guid Guid,
      string Name,
      List<UpdateLaneDto> Lanes,
       Guid LocationGuid
);

