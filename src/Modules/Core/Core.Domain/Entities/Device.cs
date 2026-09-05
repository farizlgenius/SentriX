using SharedKernel.Enums;
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
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int LocationId { get; private set; }
  public List<DeviceModule> DeviceModules { get; private set; } = default!;

  public Device(
   string Name,
   string SerialNumber,
   string Mac,
   string Ip,
   int Port,
   string Fw,
   Vendor Vendor,
   string Metadata,
   int LocationId,
   List<DeviceModule> deviceModules
   ) : base(Guid.NewGuid())
  {
    ValidationHelper.Name(Name);
    ValidationHelper.NotMinus(LocationId, nameof(LocationId));
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
    this.LocationId = LocationId;
    this.DeviceModules = deviceModules;
  }

  public Device(
    Guid Guid,
    string Name,
    string SerialNumber,
    string Mac,
    string Ip,
    int Port,
    string Fw,
    Vendor Vendor,
    string Metadata,
    int LocationId,
    List<DeviceModule> deviceModules
    ) : base(Guid)
  {
    ValidationHelper.Name(Name);
    ValidationHelper.NotMinus(LocationId, nameof(LocationId));
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
    this.LocationId = LocationId;
    this.DeviceModules = deviceModules;
  }


}