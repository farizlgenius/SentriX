namespace Core.Contract.DTOs.Department;

public sealed record DepartmentDto(
  Guid Guid,
  string Name,
  string Description,
  Guid CompanyGuid,
  string Company,
  bool IsActive,
  bool IsDefault
);