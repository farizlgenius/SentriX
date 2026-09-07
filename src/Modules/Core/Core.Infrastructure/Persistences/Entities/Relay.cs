namespace Core.Infrastructure.Persistences.Entities;

public sealed class Relay : BaseEntity
{
  public int slot_no { get; set; }
  // public OutputMode mode { get; set; }
  public string metadata { get; set; } = string.Empty;
  // Relation
  public int device_module_id { get; set; }
  public DeviceModule device_module { get; set; } = default!;
  public Relay() { }
}