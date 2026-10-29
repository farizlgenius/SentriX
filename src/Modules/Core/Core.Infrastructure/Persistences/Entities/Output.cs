using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Output : BaseEntity,IAuditableEntity
{

  public short slot_no { get; set; }
  public Vendor vendor { get; set; } = Vendor.aero;
  public string metadata { get; set; } = string.Empty;

  // Relation
  public int device_module_id { get; set; }
  public DeviceModule device_module { get; set; } = default!;
  public Output() { }
  public Output(Domain.Entities.Output d) : base(d.Guid)
  {
    name = d.Name;
    slot_no = d.SlotNo;
    vendor = d.Vendor;
    metadata = d.Metadata;
    location_id = d.LocationId;
    device_module_id = d.DeviceModuleId;
  }
}