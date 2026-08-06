

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

  public Guid DeviceGuid { get; set; }
  public Module(
    Guid Guid,
    string Name,
    string SerialNumber,
    string Fw,
    string Mac,
    int Port,
    int Address,
    string Model,
    Guid LocationGuid,
    bool IsActive,
    bool IsDefault) : base(Guid, LocationGuid, IsActive, IsDefault)
  {
    // Validate required fields
    ValidationHelper.IsNullOrEmpty(Name, nameof(Name));

    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Fw = Fw;
    this.Mac = Mac;
    this.Port = Port;
    this.Address = Address;
    this.Model = Model;
  }
}