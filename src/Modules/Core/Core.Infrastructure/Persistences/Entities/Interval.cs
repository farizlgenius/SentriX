using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Interval : BaseEntity
{
  public TimeOnly start_time { get; set; }
  public TimeOnly end_time { get; set; }

  // Releation
  public int day_id { get; set; }
  public DayInWeek day { get; set; } = default!;
  public ICollection<TimeZoneInterval> timezone_intervals { get; set; } = default!;
  public int location_id { get; set; }
  public Location location { get; set; } = default!;
  public Interval() { }
  public Interval(
    Domain.Entities.Interval d
  ) : base(d.Guid)
  {
    start_time = d.StartTime;
    end_time = d.EndTime;
    day = new DayInWeek(d.Days);
    location_id = d.LocationId;
  }
}