using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Devices : BaseDomain
{

  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public string Mac { get; private set; } = string.Empty;
  public string Ip {get; private set;} = string.Empty;
  public int Port {get; set;}
  public string Fw {get; set;} = string.Empty;
  public string Type { get; private set; }
  public string Status {get; private set;}
  public DateTime SyncedAt {get; private set;}
  public string Metadata { get; private set; } = string.Empty;

  public Devices(int id,short componetId,string name, string serialnumber, string mac,string ip,int port,string fw,string type,string status,DateTime synced_at,int locationid,string metadata,bool isactive) : base(id,componetId,locationid,isactive)
  {
    ValidationHelper.ValidateNotMinus(id, nameof(Id));
    ValidationHelper.IsValidName(name);
    ValidationHelper.IsNullOrEmpty(serialnumber, nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(mac, nameof(Mac));
    ValidationHelper.ValidateNotMinus(locationid, nameof(LocationId));
    this.Id = id;
    this.ComponentId = componetId;
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
    this.Metadata = metadata;
  }
}

