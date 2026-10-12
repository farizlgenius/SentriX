namespace Core.Contract.DTOs.Trigger;

public sealed record CreateTriggerDto(
      Guid Guid,
      string Name
);