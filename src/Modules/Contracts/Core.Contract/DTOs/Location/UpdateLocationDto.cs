namespace Core.Contract.DTOs.Location;

public sealed record UpdateLocationDto(
  Guid Guid,
  string Name,
  string Description,
  BaseDto Country
);