namespace Core.Contract.DTOs.Department;

public sealed record CreateDepartmentDto(
  string Name,
  string Description,
  Guid CompanyGuid
);