namespace Core.Contract.DTOs.Position;

public sealed record PositionDto(
      Guid Guid,
      string Name,
      string Description,
      bool IsActive,
      bool IsDefault
);