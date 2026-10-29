namespace Core.Contract.DTOs.Area;

public sealed record AreaStatusDto(
      Guid Guid,
      bool IsEnabled,
      bool IsMultiOcc,
      bool IsConfig,
      short Occ
);