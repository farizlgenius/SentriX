using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record UpdateActionDto(
      Guid Guid,
      string Name,
      Guid DeviceGuid,
      ActionType Type,
      Guid? SrcGuid = null,
      string? Metadata = null
);