using System;

namespace Location.Contract.DTOs;

public sealed record UpdateLocationDto(int Id,string Name, string Description, int CountryId);