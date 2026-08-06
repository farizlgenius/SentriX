namespace Core.Contract.DTOs.Location;

public sealed record CreateLocationDto(
  string Name,
  string Description,
  int CountryId
);