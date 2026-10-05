

using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class DeviceModule : BaseEntity,IAuditableEntity
{
  public string name { get; set; } = string.Empty;
  public string serial_number { get; set; } = string.Empty;
  public string firmware { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public int port { get; set; }
  public int address { get; set; }
  public DeviceModuleModel model { get; set; } = DeviceModuleModel.x100;
  public int reader_slot { get; set; }
  public int output_slot { get; set; }
  public int input_slot { get; set; }

  // Relation

  public int device_id { get; set; } = default!;
  public Device device { get; set; } = default!;

  public int location_id { get; set; } = default!;
  public Location location { get; set; } = default!;

  public ICollection<Reader> readers { get; set; } = default!;
  public ICollection<Sensor> sensors { get; set; } = default!;
  public ICollection<Relay> relays { get; set; } = default!;
  public ICollection<Rex> rexes { get; set; } = default!;
  public ICollection<Buzzer> buzzers { get; set; } = default!;
  public ICollection<Output> outputs { get; set; } = default!;
  public ICollection<Input> inputs { get; set; } = default!;
  public ICollection<BreakGlass> break_glasses { get; set; } = default!;

  public DeviceModule() { }
  public DeviceModule(Core.Domain.Entities.DeviceModule d) : base(d.Guid)
  {
    name = d.Name;
    serial_number = d.SerialNumber;
    firmware = d.Firmware;
    mac = d.Mac;
    model = d.Model;
    reader_slot = d.ReaderSlot;
    address = d.Address;
    input_slot = d.InputSlot;
    output_slot = d.OutputSlot;
    location_id = d.LocationId;
  }
}