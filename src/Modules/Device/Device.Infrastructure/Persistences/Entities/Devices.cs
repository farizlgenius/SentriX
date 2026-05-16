using System;
using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Devices : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public string serial_number { get; set; } = string.Empty;
  public string mac { get; set; } = string.Empty;
  public string ip { get; set; } = string.Empty;
  public int port { get; set; }
  public string fw { get; set; } = string.Empty;
  public string status { get; set; }  = string.Empty;
  public string type { get; set; } = string.Empty;
  public DateTime synced_at { get; set; }
  public string metadata { get; set; } = string.Empty;
  public int location_id { get; set; }
  public ICollection<Module> modules {get;set;} = new List<Module>();


  public Devices() { }

  public Devices(Device.Domain.Entities.Devices domain)
  {
    this.name = domain.Name;
    this.serial_number = domain.SerialNumber;
    this.mac = domain.Mac;
    this.ip = domain.Ip;
    this.port = domain.Port;
    this.fw = domain.Fw;
    this.type = domain.Type;
    this.status = domain.Status;
    this.location_id = domain.LocationId;
    this.synced_at = domain.SyncedAt;
    this.metadata = domain.Metadata;
    this.updated_at = DateTime.UtcNow;
    this.created_at = DateTime.UtcNow;
  }

  public void Update(Device.Domain.Entities.Devices domain)
  {
    this.name = domain.Name;
    this.serial_number = domain.SerialNumber;
    this.mac = domain.Mac;
    this.ip = domain.Ip;
    this.port = domain.Port;
    this.fw = domain.Fw;
    this.type = domain.Type;
    this.status = domain.Status;
    this.location_id = domain.LocationId;
    this.metadata = domain.Metadata;
    this.synced_at = domain.SyncedAt;
    this.updated_at = DateTime.UtcNow;
    this.created_at = DateTime.UtcNow;
  }

  public void UpdateMemoryAllocateStatus(string Status)
  {
    this.status = Status;
    this.updated_at = DateTime.UtcNow;
  }

  public void UpdateIp(string ip)
  {
    this.ip = ip;
    this.updated_at = DateTime.UtcNow;
  }

  public void UpdateMac(string mac)
  {
    this.mac = mac;
    this.updated_at = DateTime.UtcNow;
  }

  public void UpdatePort(int port)
  {
    this.port = port;
    this.updated_at = DateTime.UtcNow;
  }


}
