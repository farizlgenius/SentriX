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
  public short ReaderSlot {get; private set;}
  public short OutputSlot {get; private set;}
  public short InputSlot {get; private set;}
  public int LocationId { get; private set; }

  public DeviceModule(
    string name,
    string serialNumber,
    string firmware,
    string mac,
    int address,
    int port,
    DeviceModuleModel model,
    short readerSlot,
    short outputSlot,
    short inputSlot,
    int locationId,
    short externalId
  ) : base(Guid.NewGuid(),externalId)
  {
    Name = name;
    SerialNumber = serialNumber;
    Port = port;
    Firmware = firmware;
    Mac = mac;
    Address = address;
    Model = model;
    ReaderSlot = readerSlot;
    OutputSlot = outputSlot;
    InputSlot = inputSlot;
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
    short readerSlot,
    short outputSlot,
    short inputSlot,
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
    ReaderSlot = readerSlot;
    OutputSlot = outputSlot;
    InputSlot = inputSlot;
    LocationId = locationId;
  }
}