using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Door : BaseEntity,IAuditableEntity
{
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
  public ICollection<Rex> rexes { get; set; } = default!;
  public int? bg_id { get; set; }
  public BreakGlass? bg { get; set; }
  public int device_id { get; set; }
  public Device device { get; set; } = default!;
  public ICollection<GroupComponent> group_components { get; set; } = default!;
  public ICollection<AlertComponent> alert_components {get; set; } = default!;
  public ICollection<AreaDoorIn> areas_in {get; set; } = default!;
  public ICollection<AreaDoorOut> areas_out {get; set; } = default!;
  public MonitorGroupComponent? monitor_group_component {get; set; }
  public Door() { }

  public Door(Domain.Entities.Door d) : base(d.Guid,d.ExternalId,d.AltrExternalId)
  {
    name = d.Name;
    vendor = d.Vendor;
    type = d.Type;
    device_id = d.DeviceId;
    metadata = d.Metadata;
    readers = d.Readers.Select(x => new Reader(x)).ToArray();
    sensor = d.Sensor == null ? null : new Sensor(d.Sensor);
    relay = d.Relay == null ? null : new Relay(d.Relay);
    buzzer = d.Buzzer == null ? null : new Buzzer(d.Buzzer);
    rexes = d.Rexes.Select(x => new Rex(x)).ToArray();
    bg = d.BG == null ? null : new BreakGlass(d.BG);
    location_id = d.LocationId;
    group_components = new List<GroupComponent>
    {
      new GroupComponent(1,1)
    };
  }


}