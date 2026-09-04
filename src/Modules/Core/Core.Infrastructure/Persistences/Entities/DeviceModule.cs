

using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class DeviceModule : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public string serial_number { get; set; } = string.Empty;
  public string firmware { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public short port { get; set; }
  public short address { get; set; }
  public DeviceModuleModel model { get; set; } = DeviceModuleModel.x100;

  // Relation

  public int device_id { get; set; } = default!;
  public Device device { get; set; } = default!;

  public int location_id { get; set; } = default!;
  public Location location { get; set; } = default!;

  public DeviceModule() { }
  public DeviceModule(Core.Domain.Entities.SubDevice d) : base(d.Guid)
  {
    name = d.Name;
    serial_number = d.SerialNumber;
    firmware = d.Firmware;
    mac = d.Mac;
    model = d.Model;
    device_id = d.DeviceId;
    location_id = d.LocationId;
  }
}