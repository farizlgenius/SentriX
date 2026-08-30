namespace Core.Contract.DTOs.Position;

public sealed record PositionDto(
      Guid Guid,
      string Name,
      string Description,
      Guid DepartmentGuid,
      string Department,
      Guid CompanyGuid,
      string Company,
      bool IsActive,
      bool IsDefault
);