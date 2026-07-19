using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class TimeZone : BaseDbEntity
{
      public string name {get; set;} = string.Empty;
      public ICollection<Interval> intervals {get; set;} = default!;

      public TimeZone()
      {
      }

      public TimeZone(Domain.Entities.TimeZone domain) : base(domain.Guid,domain.ComponentId,domain.LocationId,domain.IsActive,domain.IsDefault)
      {
            this.name = domain.Name;
            this.intervals = domain.Intervals.Select(x => new Interval(
                  Guid.NewGuid(),
                  x.ComponentId,
                  new DayInWeek(
                        Guid.NewGuid(),
                        x.Days.Sunday,
                        x.Days.Monday,
                        x.Days.Tuesday,
                        x.Days.Wednesday,
                        x.Days.Thursday,
                        x.Days.Friday,
                        x.Days.Saturday
                  ),
                  x.Start,
                  x.End
                  )).ToList();
      }

      public void Update(Domain.Entities.TimeZone domain) 
      {
            this.name = domain.Name;
            this.intervals = domain.Intervals.Select(x => new Interval(
                  Guid.NewGuid(),
                  x.ComponentId,
                  new DayInWeek(
                        Guid.NewGuid(),
                        x.Days.Sunday,
                        x.Days.Monday,
                        x.Days.Tuesday,
                        x.Days.Wednesday,
                        x.Days.Thursday,
                        x.Days.Friday,
                        x.Days.Saturday
                  ),
                  x.Start,
                  x.End
                  )).ToList();
            this.updated_at = DateTime.UtcNow;
      }




}