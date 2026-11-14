using SharedKernel.Enums;

namespace Core.Contract.DTOs.Trigger;

public sealed record TriggerDto(
      Guid Guid,
      string Name,
      TriggerCommand Command,
      BaseDto Device,
      BaseDto TriggerSource,
      BaseDto TriggerType,
      IEnumerable<BaseDto> TriggerCodes,
      IEnumerable<BaseDto> TriggerArgs,
      IEnumerable<BaseDto>? TriggerVariables,
      BaseDto TimeZone,
      BaseDto Prcedure,
      BaseDto Src,
      bool IsActive=true,
      bool IsDefault=false
);