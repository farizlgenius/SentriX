using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public class Location
{
  public Guid Guid { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public int CountryId { get; private set; }
  public bool IsActive { get; private set; } = true;
  public bool IsDefault { get; private set; } = false;

  public Location(
    string Name,
    string Description,
    int CountryId,
    Guid Guid
  )
  {
    // Validate required fields
    ValidationHelper.IsNullOrEmpty(Name, nameof(Name));
    ValidationHelper.ValidateNotMinus(CountryId, nameof(CountryId));

    this.Name = Name;
    this.Description = Description;
    this.CountryId = CountryId;
    this.Guid = Guid;
  }
}