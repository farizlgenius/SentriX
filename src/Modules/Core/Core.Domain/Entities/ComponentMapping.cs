namespace Core.Domain.Entities;

public sealed class ComponentMappping : BaseDomain
{
  public string Entity { get; private set; } = string.Empty;
  public int InternalId { get; private set; }
  public int ExternalId { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public string Vendor {get; private set;} = string.Empty;
  public int LocationId { get; private set; }

  public ComponentMappping(
    string entity,
    int @internal,
    int external,
    string mac,
    string vendor,
    int locationId
  )
  {
    Entity = entity;
    InternalId = @internal;
    ExternalId = external;
    Mac = mac;
    Vendor = vendor;
    LocationId = locationId;
  }
  public ComponentMappping(
    Guid guid,
    string entity,
    int @internal,
    int external,
    string mac,
     string vendor,
    int locationId
    ) : base(guid)
  {
    Entity = entity;
    InternalId = @internal;
    ExternalId = external;
    Mac = mac;
    Vendor = vendor;
    LocationId = locationId;
  }

}