using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Sensor : BaseEntity
{
  public int slot_no { get; set; }
  public InputMode mode { get; set; }
  public string metadata { get; set; } = string.Empty;
  public Vendor vendor { get; set; } = Vendor.aero;
  // Relation
  public int? door_id { get; set; }
  public Door? door { get; set; }
  public int? lane_id { get; set; }
  public Lane? lane { get; set; }
  public int device_module_id { get; set; }
  public DeviceModule device_module { get; set; } = default!;
  public Sensor()
  { }

  public Sensor(
   Domain.Entities.Sensor d
 )
  {
    slot_no = d.SlotNo;
    mode = d.InputMode;
    metadata = d.Metadata;
    vendor = d.Vendor;
    device_module_id = d.DeviceModuleId;
  }
}