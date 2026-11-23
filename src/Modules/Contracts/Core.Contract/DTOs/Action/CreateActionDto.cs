using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record CreateActionDto(
      string Name,
      Guid DeviceGuid,
      ActionType Type,
      IEnumerable<Guid>? TargetGuids = null,
      string? Metadata = null
);