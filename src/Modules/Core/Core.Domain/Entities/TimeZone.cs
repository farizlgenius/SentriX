using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class TimeZone : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public List<int> IntervalIds { get; private set; } = default!;
  public int LocationId { get; private set; }
  public TimeZone(
    string name,
    List<int> intervals,
    int locationId
  ) : base(Guid.NewGuid())
  {
    ValidationHelper.Name(name);
    Name = name;
    IntervalIds = intervals;
    LocationId = locationId;
  }

  public TimeZone(
    Guid Guid,
    string name,
    List<int> intervals,
    int locationId
  ) : base(Guid)
  {
    ValidationHelper.Name(name);
    Name = name;
    IntervalIds = intervals;
    LocationId = locationId;
  }
}