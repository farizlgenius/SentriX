namespace Core.Contract.DTOs.Position;

public sealed record PositionDto(
      Guid Guid,
      string Name,
      string Description,
      string Department,
      string Company,
      bool IsActive,
      bool IsDefault
);