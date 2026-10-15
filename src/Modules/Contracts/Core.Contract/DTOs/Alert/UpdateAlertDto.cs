using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record UpdateAlertDto(
      Guid Guid,
      string Name,
      string Advice,
      Guid AlertLevelGuid,
      List<Guid> BaseEventGuid,
      Guid LocationGuid
);