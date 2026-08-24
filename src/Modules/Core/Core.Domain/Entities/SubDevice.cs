

using Core.Domain.Entities;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public class SubDevice : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public string Firmware { get; private set; } = string.Empty;
  public string Mac { get; private set; } = string.Empty;
  public int Port { get; private set; }
  public int Address { get; private set; }
  public string Model { get; private set; } = string.Empty;
  public int LocationId { get; private set; } = default!;
  public int DeviceId { get; private set; } = default!;

  public SubDevice(
    string Name,
    string SerialNumber,
    string Fw,
    string Mac,
    int Port,
    int Address,
    string Model,
    int LocationId,
    int DeviceId
    )
  {
    // Validate required fields
    ValidationHelper.Name(Name);
    ValidationHelper.NotMinus(LocationId, nameof(LocationId));
    ValidationHelper.IsNullOrEmpty(SerialNumber, nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac, nameof(Mac));
    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Firmware = Fw;
    this.Mac = Mac;
    this.Port = Port;
    this.Address = Address;
    this.Model = Model;
    this.LocationId = LocationId;
    this.DeviceId = DeviceId;
  }
  public SubDevice(
    Guid Guid,
    string Name,
    string SerialNumber,
    string Fw,
    string Mac,
    int Port,
    int Address,
    string Model,
    int LocationId,
    int DeviceId
    ) : base(Guid)
  {
    // Validate required fields
    ValidationHelper.Name(Name);
    ValidationHelper.NotMinus(LocationId, nameof(LocationId));
    ValidationHelper.IsNullOrEmpty(SerialNumber, nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac, nameof(Mac));

    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Firmware = Fw;
    this.Mac = Mac;
    this.Port = Port;
    this.Address = Address;
    this.Model = Model;
    this.LocationId = LocationId;
    this.DeviceId = DeviceId;
  }
}