

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Module : BaseEntity
{
  public string name {get; set;} = string.Empty;
  public string serial_number { get; set; } = string.Empty;
  public string fw { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public short port { get; set; }
  public short address { get; set; }
  public string model { get; set; } = string.Empty;

  // Relation

  public Guid device_guid { get; set; } = default!;
  public Device device { get; set; } = default!;

  public Guid location_guid {get; set;} = default!;
  public Location location {get; set;} = default!;

  public Module() { }
  public Module(Core.Domain.Entities.Module d) : base(d.Guid)
  {
    name = d.Name;
    serial_number = d.SerialNumber;
    fw = d.Fw;
    mac = d.Mac;
    model = d.Model;
    device_guid = d.DeviceGuid;
    location_guid = d.LocationGuid;
  }
}