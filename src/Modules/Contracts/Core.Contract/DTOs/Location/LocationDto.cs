namespace Core.Contract.DTOs.Location;

public sealed record LocationDto(
  Guid Guid,
  string Name,
  string Description,
  BaseDto Country,
  bool IsActive,
  bool IsDefault
);