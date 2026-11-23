using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record ActionDto(
      Guid Guid,
      string Name,
      Guid DeviceGuid,
      string DeviceName,
      ActionType Type,
      string ActionType,
      short Sequence,
      IEnumerable<ActionTargetDto?>? ActionTarget = null,
      IEnumerable<Guid>? TargetGuids = null,
      string? Metadata = null,
      bool IsActive=true,
      bool IsDefault=false
);