using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record UpdateAlertDto(
      Guid Guid,
      string Name,
      string Advice,
      Guid LocationGuid,
      Guid AlertLevelGuid,
      Guid CoreEntityGuid,
      Guid CoreEventGuid,
      List<Guid> Components
);