using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record UpdateAlertDto(
      EntityType Entity,
      AlertLevelDto Level,
      string Detail
);