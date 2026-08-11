namespace Core.Contract.DTOs.Company;

public sealed record CreateCompanyDto(
  string Name,
  string Address,
  string Description,
  Guid LocationGuid
);