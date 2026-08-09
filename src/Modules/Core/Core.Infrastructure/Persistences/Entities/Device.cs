

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Device : BaseEntity
{
  public string serial_number { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public string ip { get; set; } = string.Empty;
  public int port { get; set; }
  public string fw { get; set; } = string.Empty;
  public string status { get; set; } = string.Empty;
  public DateTime synced_at { get; set; }
  public string metadata { get; set; } = string.Empty;
  public ICollection<Module> modules { get; set; } = default!;

  public Device() { }
  public Device(Core.Domain.Entities.Device domain) : base(domain.Guid, domain.LocationGuid, domain.IsActive, domain.IsDefault)
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
    this.updated_at = DateTime.UtcNow;
    this.created_at = DateTime.UtcNow;
  }
}