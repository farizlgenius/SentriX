using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record AlertDto(
      Guid Guid,
      EntityType Entity,
      AlertLevelDto Level,
      string Detail
);