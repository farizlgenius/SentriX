

using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Device : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public string serial_number { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public string ip { get; set; } = string.Empty;
  public int port { get; set; }
  public string firmware { get; set; } = string.Empty;
  public SharedKernel.Enums.Vendor vendor { get; set; } = SharedKernel.Enums.Vendor.aero;
  public DeviceConfigurationStatus configuration_status { get; set; } = DeviceConfigurationStatus.pending;
  public DateTime synced_at { get; set; }
  public string metadata { get; set; } = string.Empty;

  // Releation
  public int? location_id { get; set; } = default!;
  public Location? location { get; set; } = default!;
  public ICollection<DeviceModule> device_module { get; set; } = default!;

  public Device() { }
  public Device(Core.Domain.Entities.Device domain) : base(domain.Guid)
  {
    this.name = domain.Name;
    this.serial_number = domain.SerialNumber;
    this.mac = domain.Mac;
    this.ip = domain.Ip;
    this.port = domain.Port;
    this.firmware = domain.Firmware;
    this.vendor = domain.Vendor;
    this.metadata = domain.Metadata;
    if (location_id != 0)
    {
      location_id = domain.LocationId;
    }
    device_module = domain.DeviceModules.Select(x => new DeviceModule(x)).ToArray();
  }



}