namespace Core.Contract.DTOs.Position;

public sealed record UpdatePositionDto(
      Guid Guid,
      string Name,
      string Description,
      Guid DepartmentGuid
);