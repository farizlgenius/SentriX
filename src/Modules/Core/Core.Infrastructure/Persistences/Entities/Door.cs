using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Door : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public Vendor vendor { get; set; } = Vendor.aero;
  public DoorType type { get; set; } = DoorType.Single;
  public string metadata { get; set; } = string.Empty;
  // Relation
  public ICollection<Reader> readers { get; set; } = default!;
  public int? sensor_id { get; set; }
  public Sensor? sensor { get; set; }
  public int? relay_id { get; set; }
  public Relay? relay { get; set; }

  public int? buzzer_id { get; set; }
  public Buzzer? buzzer { get; set; }
  public int? rex_id { get; set; }
  public Rex? rex { get; set; }
  public int device_module_id { get; set; }
  public DeviceModule device_module { get; set; } = default!;
  public int location_id { get; set; }
  public Location location { get; set; } = default!;
  public int? lane_id { get; set; }
  public Lane? lane { get; set; }
  public ICollection<GroupComponent> group_components { get; set; } = default!;
  public Door() { }

  public Door(Domain.Entities.Door d)
  {
    name = d.Name;
    vendor = d.Vendor;
    type = d.Type;
    metadata = d.Metadta;
    readers = d.Readers.Select(x => new Reader(x)).ToArray();
    sensor = d.Sensor == null ? null : new Sensor(d.Sensor);
    relay = d.Relay == null ? null : new Relay(d.Relay);
    buzzer = d.Buzzer == null ? null : new Buzzer(d.Buzzer);
    rex = d.Rex == null ? null : new Rex(d.Rex);
    device_module_id = d.DeviceModuleId;
    location_id = d.LocationId;
  }


}