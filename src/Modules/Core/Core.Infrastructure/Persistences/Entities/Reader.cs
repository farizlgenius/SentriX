using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Reader : BaseEntity
{
  public int slot_no { get; set; }
  public ReaderMode mode { get; set; } = ReaderMode.Osdp;
  public string metadata { get; set; } = string.Empty;
  public Vendor vendor { get; set; } = Vendor.aero;
  public ReaderDirection reader_direction { get; set; } = ReaderDirection.In;
  // Relation
  public int door_id { get; set; }
  public Door door { get; set; } = default!;
  public Reader() { }
  public Reader(
    Domain.Entities.Reader d
  )
  {
    slot_no = d.SlotNo;
    mode = d.ReaderMode;
    metadata = d.Metadata;
    vendor = d.Vendor;
    reader_direction = d.ReaderDirection;
  }
}