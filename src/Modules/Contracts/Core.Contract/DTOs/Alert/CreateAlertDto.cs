using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record CreateAlertDto(
      string Name,
      string Advice,
      Guid AlertLevelGuid,
      List<Guid> BaseEventGuid,
      Guid LocationGuid
);