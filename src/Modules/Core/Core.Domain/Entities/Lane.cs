namespace Core.Domain.Entities;

public sealed class Lane : BaseDomain
{
  public int LaneNo { get; set; }
  public List<Reader> Readers { get; set; } = default!;
  public Sensor? Sensor { get; set; }
  public Lane(
    int laneNo,
    List<Reader> readers,
    Sensor? sensor

  ) : base(Guid.NewGuid())
  {
    LaneNo = laneNo;
    Readers = readers;
    Sensor = sensor;
  }

  public Lane(
    Guid guid,
    int laneNo,
    List<Reader> readers,
    Sensor? sensor
  ) : base(guid)
  {
    LaneNo = laneNo;
    Readers = readers;
    Sensor = sensor;
  }
}