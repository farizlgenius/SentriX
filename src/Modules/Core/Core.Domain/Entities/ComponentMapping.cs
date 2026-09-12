using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class ComponentMappping : BaseDomain
{
  public string Entity { get; private set; } = string.Empty;
  public int InternalId { get; private set; }
  public int ExternalId { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int LocationId { get; private set; }

  public ComponentMappping(
    string entity,
    int @internal,
    int external,
    string mac,
    Vendor vendor,
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
     Vendor vendor,
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