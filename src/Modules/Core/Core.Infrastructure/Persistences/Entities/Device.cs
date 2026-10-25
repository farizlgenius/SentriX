

using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Device : BaseEntity,IAuditableEntity
{
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
  public ICollection<DeviceModule> device_modules { get; set; } = default!;
  public ICollection<Door> doors {get; set;} = default!;
  public ICollection<Procedure> procedures {get; set;} = default!;
  public ICollection<Persistences.Entities.Action> actions {get; set;} = default!;

  public Device() { }
  public Device(Core.Domain.Entities.Device domain) : base(domain.Guid,domain.ExternalId,domain.AltrExternalId)
  {
    this.name = domain.Name;
    this.serial_number = domain.SerialNumber;
    this.mac = domain.Mac;
    this.ip = domain.Ip;
    this.port = domain.Port;
    this.firmware = domain.Firmware;
    this.vendor = domain.Vendor;
    this.metadata = domain.Metadata;
    location_id = domain.LocationId;
    device_modules = [.. domain.DeviceModules.Select(x => new DeviceModule(x))];
  }



}