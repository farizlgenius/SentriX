using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Door : BaseEntity,IAuditableEntity
{
  public Vendor vendor { get; set; } = Vendor.aero;
  public DoorType type { get; set; } = DoorType.Single;
  public string metadata { get; set; } = string.Empty;
  public DoorExtendFeatureType extended_feature_type {get; set; } = DoorExtendFeatureType.NONE;
  // Relation
  public ICollection<Reader> readers { get; set; } = default!;
  public int? sensor_id { get; set; }
  public Sensor? sensor { get; set; }
  public int? relay_id { get; set; }
  public Relay? relay { get; set; }
  public int? follower_relay_id { get; set; }
  public FollowerRelay? follower_relay { get; set; }
  public int? buzzer_id { get; set; }
  public Buzzer? buzzer { get; set; }
  public ICollection<Rex> rexes { get; set; } = default!;
  public int? bg_id { get; set; }
  public BreakGlass? bg { get; set; }
  public int device_id { get; set; }
  public Device device { get; set; } = default!;
  public ICollection<GroupComponent> group_components { get; set; } = default!;
  public ICollection<AlertComponent> alert_components {get; set; } = default!;
  public ICollection<MonitorGroupComponent> monitor_group_components {get; set; } = default!;
  public int? from_area_id {get; set; }
  public Area? from_area {get; set; }
  public AntipassBackMode antipassback_from {get; set; } = AntipassBackMode.NONE;
  public int? to_area_id {get; set; }
  public Area? to_area {get; set; }
  public AntipassBackMode antipassback_to {get; set; }= AntipassBackMode.NONE;
  public int? interior_push_button_id {get; set; } 
  public InteriorPushButton? interior_push_button {get; set; } 
  public int? interior_push_button_output_id {get; set; } 
  public InteriorPushButtonOutput? interior_push_button_output {get; set; } 

  public Door() { }

  public Door(Domain.Entities.Door d) : base(d.Guid,d.ExternalId,d.AltrExternalId)
  {
    name = d.Name;
    vendor = d.Vendor;
    type = d.Type;
    device_id = d.DeviceId;
    metadata = d.Metadata;
    readers = [.. d.Readers.Select(x => new Reader(x))];
    sensor = d.Sensor == null ? null : new Sensor(d.Sensor);
    relay = d.Relay == null ? null : new Relay(d.Relay);
    follower_relay = d.FollowerRelay == null ? null : new FollowerRelay(d.FollowerRelay);
    buzzer = d.Buzzer == null ? null : new Buzzer(d.Buzzer);
    rexes = [.. d.Rexes.Select(x => new Rex(x))];
    bg = d.BG == null ? null : new BreakGlass(d.BG);
    location_id = d.LocationId;
    group_components =
    [
      new(1,1)
    ];
    extended_feature_type = d.ExtendFeatureType;
  }


}