using System.ComponentModel.DataAnnotations;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class ComponentMapping : BaseEntity
{
  public EntityType entity { get; set; } 
  public int? external_id { get; set; }
  public string? mac { get; set; } 
  public SharedKernel.Enums.Vendor? vendor { get; set; } = SharedKernel.Enums.Vendor.aero;

  // Relation
  public int? location_id { get; set; }
  public Location? location { get; set; } = default!;
  public ComponentMapping() { }
  public ComponentMapping(Core.Domain.Entities.ComponentMappping d) : base(d.Guid)
  {
    entity = d.Entity;
    external_id = d.ExternalId;
    if(!string.IsNullOrWhiteSpace(d.Mac))
      mac = d.Mac;
    if(d.Vendor == null)
    vendor = d.Vendor;
    if(d.LocationId == 0)
      location_id = d.LocationId;
  }

}