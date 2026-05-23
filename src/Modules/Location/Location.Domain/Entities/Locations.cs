using System;
using SharedKernel.Helpers;

namespace Location.Domain.Entities;

public sealed class Locations
{
  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public int CountryId { get; set; }

  public Locations(int id, string name, int countryId, string description)
  {
    ValidationHelper.ValidateNotMinus(id, nameof(Id));
    ValidationHelper.IsNullOrEmpty(name, nameof(name));
    Id = id;
    Name = name;
    ValidationHelper.ValidateNotMinus(countryId, nameof(CountryId));
    CountryId = countryId;
    if (!string.IsNullOrWhiteSpace(description))
    {
      Description = description.Trim();
    }
      
  }
}