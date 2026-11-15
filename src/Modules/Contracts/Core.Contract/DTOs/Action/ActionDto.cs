using SharedKernel.Enums;

namespace Core.Contract.DTOs.Action;

public sealed record ActionDto(
      Guid Guid,
      string Name,
      Guid DeviceGuid,
      string DeviceName,
      ActionType Type,
      Guid? SrcGuid = null,
      string? Metadata = null,
      bool IsActive=true,
      bool IsDefault=false
);