using Adapter.Abstraction.Constants;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Event : BaseEntity
{
  public DateTime timestamp { get; set; }
  public string actor { get; set; } = string.Empty;
  public string module { get; set; } = string.Empty;
  public string event_type { get; set; } = string.Empty;
  public string image_name { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public string component_name { get; set; } = string.Empty;
  public string event_code { get; set; } = string.Empty;
  public string remarks { get; set; } = string.Empty;
  public string capture_image_name { get; set; } = string.Empty;
  public SharedKernel.Enums.Vendor vendor { get; set; } = SharedKernel.Enums.Vendor.aero;

  // releation
  public int location_id { get; set; }
  public Location location { get; set; } = default!;

  public Event() { }

  public Event(Domain.Entities.Event d) : base(d.Guid)
  {
    timestamp = d.Timestampe;
    actor = d.Actor;
    module = d.Module;
    event_type = d.EventType;
    image_name = d.ImageName;
    mac = d.Mac;
    component_name = d.ComponentName;
    event_code = d.EventCode;
    remarks = d.Remarks;
    location_id = d.LocationId;
    capture_image_name = d.CaptureImageName;
    vendor = d.Vendor;
  }

  public Event(
    Guid guid,
        DateTime timestamp,
        string actor,
        string module,
        string eventType,
        string image_name,
        string mac,
        string component_name,
        int location_id,
        SharedKernel.Enums.Vendor vendor,
        string event_code = "",
        string remarks = "",
        string capture_image_name = ""

  ) : base(guid)
  {
    this.timestamp = timestamp;
    this.actor = actor;
    this.module = module;
    event_type = eventType;
    this.image_name = image_name;
    this.mac = mac;
    this.component_name = component_name;
    this.event_code = event_code;
    this.remarks = remarks;
    this.location_id = location_id;
    this.capture_image_name = capture_image_name;
    this.vendor = vendor;
  }

}