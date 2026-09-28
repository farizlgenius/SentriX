using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class ComponentMappping : BaseDomain
{
  public string Entity { get; private set; } = string.Empty;
  public int ExternalId { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public Vendor? Vendor { get; private set; } 
  public int LocationId { get; private set; }


  public ComponentMappping(
    Guid guid,
    string entity,
    int external,
    string mac,
    int locationId,
     Vendor? vendor = null
    ) : base(guid)
  {
    Entity = entity;
    ExternalId = external;
    Mac = mac;
    LocationId = locationId;
    Vendor = vendor;
  }

}