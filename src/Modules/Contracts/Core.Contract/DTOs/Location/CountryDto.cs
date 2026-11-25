namespace Core.Contract.DTOs.Location;

public sealed record CountryDto(
      Guid Guid,
      string Name,
      string Code
);