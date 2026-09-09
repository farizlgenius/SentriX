namespace Core.Domain.Entities;

public sealed class Lane : BaseDomain
{
  public int LaneNo { get; set; }
  public List<Reader> Readers { get; set; } = default!;
  public Sensor? Sensor { get; set; }
  public List<Relay> Relays { get; set; } = default!;
  public Lane(
    int laneNo,
    List<Reader> readers,
    Sensor? sensor,
    List<Relay> relays
  ) : base(Guid.NewGuid())
  {
    LaneNo = laneNo;
    Readers = readers;
    Sensor = sensor;
    Relays = relays;

  }

  public Lane(
    Guid guid,
    int laneNo,
    List<Reader> readers,
    Sensor? sensor,
    List<Relay> relays
  ) : base(guid)
  {
    LaneNo = laneNo;
    Readers = readers;
    Sensor = sensor;
    Relays = relays;
  }
}