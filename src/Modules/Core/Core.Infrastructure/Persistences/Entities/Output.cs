using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Output : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public int slot_no { get; set; }
  public Vendor vendor { get; set; } = Vendor.aero;
  public OutputMode mode { get; set; } = OutputMode.NC;
  public string metadata { get; set; } = string.Empty;

  // Relation
  public int location_id { get; set; }
  public Location location { get; set; } = default!;
  public int device_module_id { get; set; }
  public DeviceModule device_module { get; set; } = default!;
  public Output() { }
  public Output(Domain.Entities.Output d) : base(d.Guid)
  {
    name = d.Name;
    slot_no = d.SlotNo;
    vendor = d.Vendor;
    mode = d.Mode;
    metadata = d.Metadata;
    location_id = d.LocationId;
    device_module_id = d.DeviceModuleId;
  }
}