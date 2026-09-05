using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class DeviceModule : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public string Firmware { get; private set; } = string.Empty;
  public int Port { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public int Address { get; private set; }
  public DeviceModuleModel Model { get; private set; } = DeviceModuleModel.x100;
  public int LocationId { get; private set; }

  public DeviceModule(
    string name,
    string serialNumber,
    string firmware,
    string mac,
    int address,
    int port,
    DeviceModuleModel model,
    int locationId
  ) : base(Guid.NewGuid())
  {
    Name = name;
    SerialNumber = serialNumber;
    Port = port;
    Firmware = firmware;
    Mac = mac;
    Address = address;
    Model = model;
    LocationId = locationId;
  }

  public DeviceModule(
    Guid guid,
    string name,
    string serialNumber,
    string firmware,
    string mac,
    int address,
    int port,
    DeviceModuleModel model,
    int locationId
  ) : base(guid)
  {
    Name = name;
    SerialNumber = serialNumber;
    Port = port;
    Firmware = firmware;
    Mac = mac;
    Address = address;
    Model = model;
    LocationId = locationId;
  }
}