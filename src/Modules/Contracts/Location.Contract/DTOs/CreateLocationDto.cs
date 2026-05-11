using System;

namespace Location.Contract.DTOs;

public sealed record CreateLocationDto(string Name, string Description, int CountryId);