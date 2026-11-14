namespace Core.Contract.DTOs.Trigger;

public sealed record TriggerVariableDto(
      Guid Guid,
      BaseDto Trigger,
      BaseDto Device,
      bool IsActive=true,
      bool IsDefault=false
);