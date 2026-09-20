using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AdapterEvent : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public int component_id { get; set; }
  public string command { get; set; } = string.Empty;
  public int tag { get; set; }
  public DateTime send_at { get; set; }
  public DateTime? received_at { get; set; }
  public string body { get; set; } = string.Empty;
  public CommandStatus status { get; set; } = CommandStatus.PENDING;
  public string reason { get; set; } = string.Empty;
  public string response { get; set; } = string.Empty;
  public SharedKernel.Enums.Vendor vendor { get; set; } = SharedKernel.Enums.Vendor.aero;


  // Relation
  public int location_id { get; set; }
  public Location location { get; set; } = default!;

  public AdapterEvent() { }

  public AdapterEvent(
    Domain.Entities.AdapterEvent d
  ) : base(d.Guid)
  {
    name = d.Name;
    mac = d.Mac;
    component_id = d.ComponentId;
    command = d.Command;
    tag = d.Tag;
    send_at = d.SendAt;
    received_at = d.ReceivedAt;
    body = d.Body;
    status = d.Status;
    reason = d.Reason;
    response = d.Response;
    vendor = d.Vendor;
    location_id = d.LocationId;
  }

  public AdapterEvent(
    Guid guid,
        string name,
        string mac,
        short component_id,
        string command,
        int tag,
        DateTime send_at,
        DateTime received_at,
        string body,
        CommandStatus status,
        string reason,
        string response,
        SharedKernel.Enums.Vendor vendor,
        int locationId) : base(guid)
  {
    this.name = name;
    this.mac = mac;
    this.component_id = component_id;
    this.command = command;
    this.tag = tag;
    this.send_at = send_at;
    this.received_at = received_at;
    this.body = body;
    this.status = status;
    this.reason = reason;
    this.response = response;
    this.vendor = vendor;
    location_id = locationId;
  }
}