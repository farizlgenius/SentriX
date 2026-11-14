namespace Core.Contract.DTOs.Trigger;

public sealed record UpdateTriggerVariableDto(
      Guid Guid,
      Guid TriggerGuid,
      Guid DeviceGuid
);