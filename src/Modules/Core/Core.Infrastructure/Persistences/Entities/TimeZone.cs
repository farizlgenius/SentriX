namespace Core.Infrastructure.Persistences.Entities;

public sealed class TimeZone : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public ICollection<TimeZoneInterval> timezone_intervals { get; set; } = default!;
  // Relation
  public int location_id { get; set; }
  public Location location { get; set; } = default!;
  public TimeZone() { }
  public TimeZone(Domain.Entities.TimeZone d)
  {
    name = d.Name;
    timezone_intervals = d.IntervalIds.Select(
      x => new TimeZoneInterval(0, x)
    ).ToList();
    location_id = d.LocationId;
  }
}