using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Reader : BaseEntity
{
  public int slot_no { get; set; }
  public ReaderMode mode { get; set; } = ReaderMode.Osdp;
  public string metadata { get; set; } = string.Empty;
  // Relation
  public int device_module_id { get; set; }
  public DeviceModule device_module { get; set; } = default!;
  public Reader()
  { }
}