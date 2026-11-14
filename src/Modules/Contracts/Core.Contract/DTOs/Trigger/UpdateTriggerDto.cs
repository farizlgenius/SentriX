using SharedKernel.Enums;

namespace Core.Contract.DTOs.Trigger;

public sealed record UpdateTriggerDto(
      Guid Guid,
      string Name,
      TriggerCommand Command,
      Guid DeviceGuid,
      Guid TriggerSource,
      Guid TriggerType,
      IEnumerable<Guid> TriggerCodes,
      IEnumerable<Guid> TriggerArgs,
      Guid? FirstTriggerVariable,
      Guid? SecondTriggerVariable,
      Guid? ThirdTriggerVariable,
      Guid? ForthTriggerVariable,
      Guid TimeZoneGuid,
      Guid PrcedureGuid,
      Guid SrcGuid,
      EntityType SrcType=EntityType.DEVICE
);