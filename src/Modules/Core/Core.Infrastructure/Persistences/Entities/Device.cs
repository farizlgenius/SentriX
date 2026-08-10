

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Device : BaseEntity
{
  public string name {get; set;} = string.Empty;
  public string serial_number { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public string ip { get; set; } = string.Empty;
  public int port { get; set; }
  public string fw { get; set; } = string.Empty;
  public string status { get; set; } = string.Empty;
  public DateTime synced_at { get; set; }
  public string metadata { get; set; } = string.Empty;

  // Releation
  public Guid location_guid {get; set;} = default!;
  public Location location {get; set;} = default!;
  public ICollection<Module> modules { get; set; } = default!;

  public Device() { }
  public Device(Core.Domain.Entities.Device domain) : base(domain.Guid)
  {
    this.name = domain.Name;
    this.serial_number = domain.SerialNumber;
    this.mac = domain.Mac;
    this.ip = domain.Ip;
    this.port = domain.Port;
    this.fw = domain.Fw;
    this.status = domain.Status;
    this.synced_at = domain.SyncedAt;
    this.metadata = domain.Metadata;
  }
}