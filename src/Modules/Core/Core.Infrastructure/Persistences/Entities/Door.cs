using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Door : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public Vendor vendor { get; set; } = Vendor.aero;
  public DoorType type { get; set; } = DoorType.Single;
  public DoorDirection direction { get; set; } = DoorDirection.In;
  public string metadata { get; set; } = string.Empty;
  // Relation
  public int? reader_id { get; set; }
  public Reader? reader { get; set; }
  public int? sensor_id { get; set; }
  public Sensor? sensor { get; set; }
  public int? relay_id { get; set; }
  public Relay? relay { get; set; }
  public int? rex_id { get; set; }
  public Rex? rex { get; set; }
  public int device_id { get; set; }
  public Device device { get; set; } = default!;
  public int location_id { get; set; }
  public Location location { get; set; } = default!;
  public Door() { }


}