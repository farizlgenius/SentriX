using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Door : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public Vendor Vendor { get; private set; }
  public DoorType Type { get; private set; }
  public string Metadta { get; private set; } = string.Empty;
  public List<Reader> Readers { get; private set; } = default!;
  public Sensor? Sensor { get; private set; }
  public Relay? Relay { get; private set; }
  public Buzzer? Buzzer { get; private set; }
  public Rex? Rex { get; private set; }
  public int LocationId { get; private set; }
  public Door(
    string name,
    Vendor vendor,
    DoorType type,
    string metadata,
   List<Reader> readers,
    Sensor? sensor,
    Relay? relay,
    Buzzer? buzzer,
    Rex? rex,
    int locationId
  ) : base(Guid.NewGuid())
  {
    Name = name;
    Vendor = vendor;
    Type = type;
    Metadta = metadata;
    Readers = readers;
    Sensor = sensor;
    Relay = relay;
    Buzzer = buzzer;
    Rex = rex;
    LocationId = locationId;
  }
  public Door(
    Guid guid,
    string name,
    Vendor vendor,
    DoorType type,
    string metadata,
  List<Reader> readers,
    Sensor? sensor,
    Relay? relay,
    Buzzer? buzzer,
    Rex? rex,
    int locationId
  ) : base(guid)
  {
    ValidationHelper.Name(name);
    ValidationHelper.Vendor(vendor);
    ValidationHelper.ReaderNumberPerDoor(readers.Count());
    Name = name;
    Vendor = vendor;
    Type = type;
    Metadta = metadata;
    Readers = readers;
    Sensor = sensor;
    Relay = relay;
    Buzzer = buzzer;
    Rex = rex;
    LocationId = locationId;
  }
}