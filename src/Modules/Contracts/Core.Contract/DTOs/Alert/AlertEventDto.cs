using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record AlertEventDto(
      Guid Guid,
      DateTime Timestamp,
      string Name,
      EntityType Entity,
      CoreEvent BaseEvent,
      AlertAction Action,
      string? Comment,
      string AlertLevel,
      string LevelColor,
      string? Username,
      bool IsActive,
      bool IsDefault
);