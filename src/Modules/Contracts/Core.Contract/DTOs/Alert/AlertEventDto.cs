using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record AlertEventDto(
      Guid Guid,
      DateTime Timestamp,
      string Name,
      EntityType Entity,
      BaseEvent BaseEvent,
      bool Acknowledge,
      bool Postpone,
      string Comment,
      string AlertLevel,
      string LevelColor,
      bool IsActive,
      bool IsDefault
);