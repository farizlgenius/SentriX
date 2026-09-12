using System.ComponentModel.DataAnnotations;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class ComponentMapping : BaseEntity
{
  public string entity { get; set; } = string.Empty;
  public int internal_id { get; set; }
  public int external_id { get; set; }
  public string mac { get; set; } = string.Empty;
  public SharedKernel.Enums.Vendor vendor { get; set; } = SharedKernel.Enums.Vendor.aero;

  // Relation
  public int location_id { get; set; }
  public Location location { get; set; } = default!;
  public ComponentMapping() { }
  public ComponentMapping(Core.Domain.Entities.ComponentMappping d) : base(d.Guid)
  {
    entity = d.Entity;
    internal_id = d.InternalId;
    external_id = d.ExternalId;
    mac = d.Mac;
    vendor = d.Vendor;
    location_id = d.LocationId;
  }

}