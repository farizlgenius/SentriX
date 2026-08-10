namespace Core.Contract.DTOs.Department;

public sealed record UpdateDepartmentDto(
  Guid Guid,
  string Name,
  string Description,
  Guid CompanyGuid
);