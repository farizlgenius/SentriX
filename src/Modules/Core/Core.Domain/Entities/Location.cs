using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public class Location : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public int CountryId { get; private set; }

  public Location(
    string Name,
    string Description,
    int CountryId,
    Guid Guid
  ) : base(Guid, Guid.Empty, true, false)
  {
    // Validate required fields
    ValidationHelper.IsNullOrEmpty(Name, nameof(Name));
    ValidationHelper.ValidateNotMinus(CountryId, nameof(CountryId));

    this.Name = Name;
    this.Description = Description;
    this.CountryId = CountryId;
  }
}