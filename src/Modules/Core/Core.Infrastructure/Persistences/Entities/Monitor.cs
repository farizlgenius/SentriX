using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Monitor : BaseEntity,IAuditableEntity
{
  public short slot_no { get; set; }
  public Vendor vendor { get; set; } = Vendor.aero;
  public string metadata { get; set; } = string.Empty;

  // Relation
  public int device_module_id { get; set; }
  public DeviceModule device_module { get; set; } = default!;
  public ICollection<MonitorGroupComponent> monitor_group_components { get; set; } = default!;
  public ICollection<Trigger> triggers {get; set; } = default!;
  public ICollection<ActionTarget> action_targets {get; set; } = default!;
  public Monitor() { }
  public Monitor(Domain.Entities.Monitor d) : base(d.Guid,d.ExternalId)
  {
    name = d.Name;
    slot_no = d.SlotNo;
    vendor = d.Vendor;
    metadata = d.Metadata;
    location_id = d.LocationId;
    device_module_id = d.DeviceModuleId;
  }
}