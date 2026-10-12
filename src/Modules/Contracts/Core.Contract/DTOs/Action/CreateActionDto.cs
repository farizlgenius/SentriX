using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record CreateActionDto(
      string Name,
      Guid DeviceGuid,
      ActionType Type,
      string Metadata,
      Guid LocationGuid
);