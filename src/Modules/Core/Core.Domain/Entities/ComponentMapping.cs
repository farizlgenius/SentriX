using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class ComponentMappping : BaseDomain
{
  public EntityType Entity { get; private set; } 
  public int ExternalId { get; private set; }
  public int DeviceId {get; private set;}


  public ComponentMappping(
    Guid guid,
    EntityType entity,
    int external,
    int deviceId
    ) : base(guid)
  {
    Entity = entity;
    ExternalId = external;
    DeviceId = deviceId;
  }

}