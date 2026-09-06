using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Interval : BaseDomain
{
  public TimeOnly StartTime { get; private set; }
  public TimeOnly EndTime { get; private set; }
  public DayInWeek Days { get; private set; } = default!;
  public int LocationId { get; private set; }
  public Interval(
    TimeOnly start,
    TimeOnly end,
    DayInWeek days,
    int locationId
  ) : base(Guid.NewGuid())
  {
    ValidationHelper.Time(StartTime, EndTime);
    StartTime = start;
    EndTime = end;
    Days = days;
    LocationId = locationId;
  }

  public Interval(
    Guid guid,
    TimeOnly start,
    TimeOnly end,
    DayInWeek days,
    int locationId
  ) : base(guid)
  {
    ValidationHelper.Time(StartTime, EndTime);
    StartTime = start;
    EndTime = end;
    Days = days;
    LocationId = locationId;
  }
}