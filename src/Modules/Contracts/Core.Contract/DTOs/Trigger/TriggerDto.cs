using SharedKernel.Enums;

namespace Core.Contract.DTOs.Trigger;

public sealed record TriggerDto(
      Guid Guid,
      string Name,
      TriggerCommand Command,
      Guid DeviceGuid,
      string Device,
      Guid TriggerSourceGuid,
      string TriggerSource,
      Guid TriggerTypeGuid,
      string TriggerType,
      IEnumerable<Guid> TriggerCodes,
      IEnumerable<Guid> TriggerArgs,
      IEnumerable<Guid>? TriggerVariables,
      Guid TimeZoneGuid,
      string TimeZone,
      Guid PrcedureGuid,
      string Procedure,
      Guid SrcGuid,
      string SrcName,
      bool IsActive=true,
      bool IsDefault=false
);