namespace Core.Contract.DTOs.Department;

public sealed record DepartmentDto(
  Guid Guid,
  string Name,
  string Description,
  Guid CompanyGuid,
  bool IsActive,
  bool IsDefault
);