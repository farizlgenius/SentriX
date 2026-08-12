using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Device : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public string Mac { get; private set; } = string.Empty;
  public string Ip { get; private set; } = string.Empty;
  public int Port { get; set; }
  public string Firmware { get; set; } = string.Empty;
  public string Metadata { get; private set; } = string.Empty;
  public string Vendor { get; private set; } = string.Empty;
  public Guid LocationGuid { get; private set; }

  public Device(
   string Name,
   string SerialNumber,
   string Mac,
   string Ip,
   int Port,
   string Fw,
   string Vendor,
   string Metadata,
   Guid LocationGuid
   )
  {
    ValidationHelper.Name(Name);
    ValidationHelper.GuidEmpty(LocationGuid, nameof(LocationGuid));
    ValidationHelper.IsNullOrEmpty(SerialNumber, nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac, nameof(Mac));
    ValidationHelper.IsNullOrEmpty(Ip, nameof(Ip));
    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Mac = Mac;
    this.Ip = Ip;
    this.Port = Port;
    this.Firmware = Fw;
    this.Metadata = Metadata;
    this.Vendor = Vendor;
    this.LocationGuid = LocationGuid;
  }

  public Device(
    Guid Guid,
    string Name,
    string SerialNumber,
    string Mac,
    string Ip,
    int Port,
    string Fw,
    string Vendor,
    string Metadata,
    Guid LocationGuid
    ) : base(Guid)
  {
    ValidationHelper.Name(Name);
    ValidationHelper.GuidEmpty(LocationGuid, nameof(LocationGuid));
    ValidationHelper.IsNullOrEmpty(SerialNumber, nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac, nameof(Mac));
    ValidationHelper.IsNullOrEmpty(Ip, nameof(Ip));
    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Mac = Mac;
    this.Ip = Ip;
    this.Port = Port;
    this.Firmware = Fw;
    this.Vendor = Vendor;
    this.Metadata = Metadata;
    this.LocationGuid = LocationGuid;
  }
}