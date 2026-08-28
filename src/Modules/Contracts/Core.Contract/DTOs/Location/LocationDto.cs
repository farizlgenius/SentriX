namespace Core.Contract.DTOs.Location;

public sealed record LocationDto(
  Guid Guid,
  string Name,
  string Description,
  int CountryId,
  string Country,
  bool IsActive,
  bool IsDefault
);