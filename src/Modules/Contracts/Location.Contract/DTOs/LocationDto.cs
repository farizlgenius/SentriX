namespace Location.Contract.DTOs;

public sealed record LocationDto(int Id, string Name, string Description, int CountryId, string Country,bool IsActive,bool IsDefault);
