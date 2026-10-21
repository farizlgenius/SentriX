using System.ComponentModel.DataAnnotations;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class ComponentMapping : BaseEntity
{
  public EntityType entity { get; set; } 
  public int? external_id { get; set; }
  // Releation
  public int? device_id {get; set; }
  public Device? device {get ;set ;}

  // Relation
  public ComponentMapping() { }
  public ComponentMapping(Core.Domain.Entities.ComponentMappping d) : base(d.Guid)
  {
    entity = d.Entity;
    external_id = d.ExternalId;
    if(d.DeviceId != 0)
      device_id = d.DeviceId;
  }

}