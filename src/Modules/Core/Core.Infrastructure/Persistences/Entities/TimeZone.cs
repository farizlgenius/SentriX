using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TimeZone : BaseEntity,IAuditableEntity
{
  public ICollection<TimeZoneInterval> timezone_intervals { get; set; } = default!;
  // Relation
  public ICollection<GroupComponent> group_components { get; set; } = default!;
  public ICollection<Rex> rexes {get; set; } = default!;
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