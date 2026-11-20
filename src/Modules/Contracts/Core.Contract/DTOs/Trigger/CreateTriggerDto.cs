using SharedKernel.Enums;

namespace Core.Contract.DTOs.Trigger;

public sealed record CreateTriggerDto(
      string Name,
      TriggerCommand Command,
      Guid Device,
      Guid TriggerSource,
      Guid TriggerType,
      IEnumerable<Guid> TriggerCodes,
      IEnumerable<Guid> TriggerArgs,
      Guid? FirstTriggerVariable,
      Guid? SecondTriggerVariable,
      Guid? ThirdTriggerVariable,
      Guid? ForthTriggerVariable,
      Guid TimeZone,
      Guid Procedure,
      Guid Src,
      EntityType SrcType=EntityType.DEVICE
);