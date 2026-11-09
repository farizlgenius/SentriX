using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record ActionDto(
      Guid Guid,
      string Name,
      string DeviceName,
      Guid DeviceGuid,
      ActionType Type,
      string Metadata,
      string LocationName,
      Guid LocationGuid,
      bool IsActive,
      bool IsDefault
);