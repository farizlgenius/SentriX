

using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Relay : BaseEntity
{
  public int slot_no { get; set; }
  public OutputMode mode { get; set; } = OutputMode.NC;
  public string metadata { get; set; } = string.Empty;
  public Vendor vendor { get; set; } = Vendor.aero;
  // Relation
  public int door_id { get; set; }
  public Door door { get; set; } = default!;
  public Relay() { }
  public Relay(
  Domain.Entities.Relay d
)
  {
    slot_no = d.SlotNo;
    metadata = d.Metadata;
    vendor = d.Vendor;
    mode = d.OutputMode;
  }
}