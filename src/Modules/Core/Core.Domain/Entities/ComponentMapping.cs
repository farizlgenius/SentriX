namespace Core.Domain.Entities;

public sealed class ComponentMappping : BaseDomain
{
  public string Entity { get; private set; } = string.Empty;
  public Guid InternalGuid { get; private set; }
  public int ExternalId { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public string Vendor {get; private set;} = string.Empty;
  public Guid LocationGuid { get; private set; }

  public ComponentMappping(
    string entity,
    Guid @internal,
    int external,
    string mac,
    string vendor,
    Guid locationGuid
  )
  {
    Entity = entity;
    InternalGuid = @internal;
    ExternalId = external;
    Mac = mac;
    Vendor = vendor;
    LocationGuid = locationGuid;
  }
  public ComponentMappping(
    Guid guid,
    string entity,
    Guid @internal,
    int external,
    string mac,
     string vendor,
    Guid locationGuid
    ) : base(guid)
  {
    Entity = entity;
    InternalGuid = @internal;
    ExternalId = external;
    Mac = mac;
    Vendor = vendor;
    LocationGuid = locationGuid;
  }

}