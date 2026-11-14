namespace Core.Contract.DTOs.Trigger;

public sealed record CreateTriggerVariableDto(
      Guid TriggerGuid,
      Guid DeviceGuid
);