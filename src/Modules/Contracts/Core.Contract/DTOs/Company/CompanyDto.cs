namespace Core.Contract.DTOs.Company;

public sealed record CompanyDto(
  Guid Guid,
  string Name,
  string Address,
  string Description,
  Guid LocationGuid,
  bool IsActive,
  bool IsDefault
);