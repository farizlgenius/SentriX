namespace Core.Domain.Entities;

public sealed class Lane : BaseDomain
{
  public int LaneNo { get; set; }
  public List<Door> Doors { get; set; } = default!;
  public Lane(
    int laneNo,
    List<Door> doors

  ) : base(Guid.NewGuid())
  {
    LaneNo = laneNo;
    Doors = doors;
  }

  public Lane(
    Guid guid,
    int laneNo,
    List<Door> doors
  ) : base(guid)
  {
    LaneNo = laneNo;
    Doors = doors;
  }
}