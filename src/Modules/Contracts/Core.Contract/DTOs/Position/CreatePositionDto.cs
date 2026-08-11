namespace Core.Contract.DTOs.Position;

public sealed record CreatePositionDto(
      string Name,
      string Description,
      Guid DepartmentGuid
);