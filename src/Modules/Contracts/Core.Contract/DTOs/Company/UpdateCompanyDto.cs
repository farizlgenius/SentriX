namespace Core.Contract.DTOs.Company;

public sealed record UpdateCompanyDto(
  Guid Guid,
  string Name,
  string Address,
  string Description,
  Guid LocationGuid
);