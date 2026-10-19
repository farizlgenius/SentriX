using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record AlertDto(
      Guid Guid,
      string Name,
      string Advice,
      Guid LocationGuid,
      string LocationName,
      Guid AlertLevelGuid,
      string AlertLevelColor,
      string AlertLevelName,
      Guid CoreEntityGuid,
      Guid CoreEventGuid,
      List<Guid> Components,
      bool IsActive,
      bool IsDefault
);