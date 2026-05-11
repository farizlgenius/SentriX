using System;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Devices
{
  public int Id { get; private set; }
  public int ComponentId { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public string Mac { get; private set; } = string.Empty;
  public string Ip {get; private set;} = string.Empty;
  public int Port {get; set;}
  public string Fw {get; set;} = string.Empty;
  public string Type { get; private set; }
  public string Status {get; private set;}
  public DateTime SyncedAt {get; private set;}
  public int LocationId { get; private set; }

  public Devices(int id,int componentid, string name, string serialnumber, string mac,string ip,int port,string fw,string type,string status,DateTime synced_at,int locationid)
  {
    ValidationHelper.ValidateNotMinus(id, nameof(Id));
    ValidationHelper.ValidateNotMinus(componentid, nameof(ComponentId));
    ValidationHelper.ValidateNotNullOrEmpty(name, nameof(Name));
    ValidationHelper.ValidateNotNullOrEmpty(serialnumber, nameof(SerialNumber));
    ValidationHelper.ValidateNotNullOrEmpty(mac, nameof(Mac));
    ValidationHelper.ValidateNotMinus(locationid, nameof(LocationId));
    ValidationHelper.ValidateNotNullOrEmpty(ip,nameof(Ip));
    this.Id = id;
    this.ComponentId = componentid;
    this.Name = name;
    this.SerialNumber = serialnumber;
    this.Mac = mac;
    this.Ip = ip;
    this.Port = port;
    this.Fw = fw;
    this.Type = type;
    this.Status = status;
    this.LocationId = locationid;
    this.SyncedAt = synced_at;
  }
}

