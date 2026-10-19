using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;


public sealed record InsertAlertEventDto(
      DateTime Timestamp,
      string Name,
      EntityType Entity,
      CoreEvent BaseEvent,
      AlertAction Action,
      string Comment,
      string AlertLevel,
      string LevelColor,
      string Username
);