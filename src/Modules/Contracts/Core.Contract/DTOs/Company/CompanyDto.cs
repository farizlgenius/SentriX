namespace Core.Contract.DTOs.Company;

public sealed record CompanyDto(
  Guid Guid,
  string Name,
  string Address,
  string Description,
  bool IsActive,
  bool IsDefault
);