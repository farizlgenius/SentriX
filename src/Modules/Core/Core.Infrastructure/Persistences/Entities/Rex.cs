using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Rex : BaseEntity
{
  public int slot_no { get; set; }
  public InputMode mode { get; set; }
  public string metadata { get; set; } = string.Empty;
  public Vendor vendor { get; set; } = Vendor.aero;
  // Relation
  public int door_id { get; set; }
  public Door door { get; set; } = default!;
  public Rex()
  { }

  public Rex(
   Domain.Entities.Rex d
 )
  {
    slot_no = d.SlotNo;
    mode = d.InputMode;
    metadata = d.Metadata;
    vendor = d.Vendor;
    door_id = d.DoorId;
  }
}