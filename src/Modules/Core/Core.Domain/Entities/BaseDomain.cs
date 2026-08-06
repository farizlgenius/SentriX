namespace Core.Domain.Entities;

public class BaseDomain
{
  public Guid Guid { get; private set; }
  public Guid LocationGuid { get; private set; }
  public bool IsActive { get; private set; } = true;
  public bool IsDefault { get; private set; } = false;

  public BaseDomain(
    Guid Guid,
    Guid LocationGuid,
    bool IsActive,
    bool IsDefault
  )
  {
    this.Guid = Guid;
    this.LocationGuid = LocationGuid;
    this.IsActive = IsActive;
    this.IsDefault = IsDefault;
  }

}