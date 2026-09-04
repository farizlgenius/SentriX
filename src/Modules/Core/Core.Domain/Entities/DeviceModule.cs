using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class DeviceModule : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public int Port { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public int Address { get; private set; }
  public DeviceModuleModel Model { get; private set; } = DeviceModuleModel.x100;

  public DeviceModule(
    string name,
    string serialNumber,
    string mac,
    int address,
    int port,
    DeviceModuleModel model
  ) : base(Guid.NewGuid())
  {
    Name = name;
    SerialNumber = serialNumber;
    Port = port;
    Mac = mac;
    Address = address;
    Model = model;
  }

  public DeviceModule(
    Guid guid,
    string name,
    string serialNumber,
    string mac,
    int address,
    int port,
    DeviceModuleModel model
  ) : base(guid)
  {
    Name = name;
    SerialNumber = serialNumber;
    Port = port;
    Mac = mac;
    Address = address;
    Model = model;
  }
}