namespace Core.Contract.DTOs.Department;

public sealed record DepartmentDto(
  Guid Guid,
  string Name,
  string Description,
  bool IsActive,
  bool IsDefault
);