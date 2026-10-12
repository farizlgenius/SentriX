using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record ActionDto(
      Guid Guid,
      string Name,
      Guid DeviceGuid,
      ActionType Type,
      string Metadata,
      bool IsActive,
      bool IsDefault
);