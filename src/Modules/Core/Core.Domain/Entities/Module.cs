

using Core.Domain.Entities;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public class Module : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public string Fw { get; private set; } = string.Empty;
  public string Mac { get; private set; } = string.Empty;
  public int Port { get; private set; }
  public int Address { get; private set; }
  public string Model { get; private set; } = string.Empty;
  public Guid LocationGuid { get; private set; } = default!;
  public Guid DeviceGuid { get; private set; } = default!;

  public Module(
    string Name,
    string SerialNumber,
    string Fw,
    string Mac,
    int Port,
    int Address,
    string Model,
    Guid LocationGuid
    )
  {
    // Validate required fields
    ValidationHelper.Name(Name);
    ValidationHelper.GuidEmpty(LocationGuid, nameof(LocationGuid));
    ValidationHelper.IsNullOrEmpty(SerialNumber, nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac, nameof(Mac));
    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Fw = Fw;
    this.Mac = Mac;
    this.Port = Port;
    this.Address = Address;
    this.Model = Model;
    this.LocationGuid = LocationGuid;
    this.DeviceGuid = DeviceGuid;
  }
  public Module(
    Guid Guid,
    string Name,
    string SerialNumber,
    string Fw,
    string Mac,
    int Port,
    int Address,
    string Model,
    Guid LocationGuid
    ) : base(Guid)
  {
    // Validate required fields
    ValidationHelper.Name(Name);
    ValidationHelper.GuidEmpty(LocationGuid, nameof(LocationGuid));
    ValidationHelper.IsNullOrEmpty(SerialNumber, nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac, nameof(Mac));

    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Fw = Fw;
    this.Mac = Mac;
    this.Port = Port;
    this.Address = Address;
    this.Model = Model;
    this.LocationGuid = LocationGuid;
    this.DeviceGuid = DeviceGuid;
  }
}