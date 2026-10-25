namespace Core.Contract.DTOs.Area;

public sealed record AreaDto(
      Guid Guid,
      string Name,
      bool IsActive,
      bool IsDefault
);