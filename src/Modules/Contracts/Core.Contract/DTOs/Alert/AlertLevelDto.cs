using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record AlertLevelDto(
      Guid Guid,
      string Name,
      string ColorCode,
      bool IsActive,
      bool IsDefault
);