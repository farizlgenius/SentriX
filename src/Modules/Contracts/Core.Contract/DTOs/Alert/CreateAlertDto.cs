using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record CreateAlertDto(
      string Name,
      string Advice,
      Guid LocationGuid,
      Guid AlertLevelGuid,
      Guid CoreEntityGuid,
      Guid CoreEventGuid,
      List<Guid> Components
);