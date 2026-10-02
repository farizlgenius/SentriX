using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class ComponentMappping : BaseDomain
{
  public EntityType Entity { get; private set; } 
  public int InternaleId {get; private set;}
  public int ExternalId { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public Vendor? Vendor { get; private set; } 
  public int LocationId { get; private set; }


  public ComponentMappping(
    Guid guid,
    EntityType entity,
    int @internal,
    int external,
    string mac,
    int locationId,
     Vendor? vendor = null
    ) : base(guid)
  {
    Entity = entity;
    InternaleId = @internal;
    ExternalId = external;
    Mac = mac;
    LocationId = locationId;
    Vendor = vendor;
  }

}