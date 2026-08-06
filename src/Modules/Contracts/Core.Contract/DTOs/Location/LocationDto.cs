namespace Core.Contract.DTOs.Location;

public sealed record LocationDto(
  Guid Guid,
  string Name,
  string Description,
  int CountryId,
  bool IsActive,
  bool IsDefault
);