using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record AlertDto(
      Guid Guid,
      string Name,
      string Advice,
      Guid LocationGuid,
      string LocationName,
      Guid AlertLevelGuid,
      string AlertLevel,
      List<BaseEventDto> BaseEventGuids
);