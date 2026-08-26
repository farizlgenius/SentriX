using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public class Location
{
  public Guid Guid { get; private set; } = default!;
  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public int CountryId { get; private set; }

  public Location(
    Guid Guid,
    string Name,
    string Description,
    int CountryId

  )
  {
    // Validate required fields
    ValidationHelper.IsNullOrEmpty(Name, nameof(Name));
    ValidationHelper.NotMinus(CountryId, nameof(CountryId));
    ValidationHelper.GuidEmpty(Guid, nameof(Guid));
    this.Guid = Guid;
    this.Name = Name;
    this.Description = Description;
    this.CountryId = CountryId;
  }

  public Location(
    string Name,
    string Description,
    int CountryId

  )
  {
    // Validate required fields
    ValidationHelper.IsNullOrEmpty(Name, nameof(Name));
    ValidationHelper.NotMinus(CountryId, nameof(CountryId));
    this.Guid = Guid.NewGuid();
    this.Name = Name;
    this.Description = Description;
    this.CountryId = CountryId;
  }
}